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
    private bool? _isAuthorized = null!;

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

    private void SetAuthorization(bool? b)
    {
        _isAuthorized = b;
    }

    private async void AuthorizeUser(object obj)
    {
        AuthorizationModel user =new AuthorizationModel() { LoginOrEmail = UserLogin, Password = UserPassword };
        HubConnection connection = App.MainHubConnectionConfiguration();
        MainHubService signal = new MainHubService(connection);
        int waitingTime = 10;

        await signal.Connect();
        await signal.SendAuthorizeMessage(user);
        signal.ReceiveAuthorizeMessage();
        signal.AuthorizationResponseArrived += SetAuthorization;

        Stopwatch stopwatch = Stopwatch.StartNew();
        while (true)
        {
            if (_isAuthorized != null || stopwatch.Elapsed.TotalSeconds >= waitingTime) break;
            Task.Delay(100).Wait();
        }
        stopwatch.Stop();
        //TODO ответом от сервера должен приходить игрок
        if (_isAuthorized == null)
        {
            MessageBox.Show("Превышено время ожидания");
        }
        else if (_isAuthorized == true)
        {
            var window = new MainView(new MainViewModel(new PlayerModel()));
            window.Show();
        }
        else
        {
            MessageBox.Show("Ошибка");
        }
        signal.AuthorizationResponseArrived -= SetAuthorization;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
