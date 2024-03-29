using Microsoft.AspNetCore.SignalR.Client;
using Quizzify.Client.Services;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;

namespace Quizzify.Client.ViewModel;

public class AuthorizationViewModel: INotifyPropertyChanged
{
    private string _userLogin;
    public string UserLogin
    {
        get => _userLogin;
        set
        {
            if (_userLogin != value)
            {
                _userLogin = value;
                OnPropertyChanged(nameof(UserLogin));
            }
        }
    }

    private string _userPassword;
    public string UserPassword
    {
        get => _userPassword;
        set
        {
            if (_userPassword != value)
            {
                _userPassword = value;
                OnPropertyChanged(nameof(UserPassword));
            }
        }
    }

    private async void RegistrationUser(object obj)
    {
        HubConnection connection = App.MainHubConnectionConfiguration();

        SignalRService signal = new SignalRService(connection);
        await signal.Connect();
        await signal.SendAuthorizeMessage(UserLogin, UserPassword);
        signal.ReceiveAuthorizeMessage();
        Stopwatch stopwatch = Stopwatch.StartNew();
        while (true)
        {
            if (signal.IsAuthorized != null || stopwatch.Elapsed.TotalSeconds >= 10) break;
            Task.Delay(100).Wait();
        }
        stopwatch.Stop();

        if (signal.IsAuthorized == null) MessageBox.Show("Превышено время ожидания");
        else if (signal.IsAuthorized == true) MessageBox.Show("Пользователь зарегистрирован!");
        else MessageBox.Show("Ошибка");
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
