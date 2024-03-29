using Microsoft.AspNetCore.SignalR.Client;
using Quizzify.Client.Model.Users;
using Quizzify.Client.Services;
using Quizzify.Client.View;
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

    private async void AuthorizeUser(object obj)
    {
        var user=new AuthorizationModel() { LoginOrEmail = UserLogin, Password = UserPassword };

        HubConnection connection = App.MainHubConnectionConfiguration();
        SignalRService signal = new SignalRService(connection);
        int waitingTime = 10;

        await signal.Connect();
        await signal.SendAuthorizeMessage(user);
        signal.ReceiveAuthorizeMessage();

        Stopwatch stopwatch = Stopwatch.StartNew();
        while (true)
        {
            if (signal.IsAuthorized != null || stopwatch.Elapsed.TotalSeconds >= waitingTime) break;
            Task.Delay(100).Wait();
        }
        stopwatch.Stop();

        if (signal.IsAuthorized == null)
        {
            MessageBox.Show("Превышено время ожидания");
        }
        else if (signal.IsAuthorized == true)
        {
            var window = new MainView(new MainViewModel());
            window.Show();
        }
        else
        {
            MessageBox.Show("Ошибка");
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
