using AutoMapper;
using Microsoft.Extensions.Configuration;
using Quizzify.Client.Mappers;
using Quizzify.Client.Command;
using System.ComponentModel;
using System.Windows.Input;

using Quizzify.Client.Security;
using Microsoft.AspNetCore.SignalR.Client;
using System.Diagnostics;
using System.Windows;
using Quizzify.Client.Services;

namespace Quizzify.Client.ViewModel;

public class RegistrationViewModel : INotifyPropertyChanged
{
    private readonly IConfiguration configuration;
    private readonly IMapper _mapper;
    public ICommand RegistrationUserCommand { get; }

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

    private string _userEmail;
    public string UserEmail
    {
        get => _userEmail;
        set
        {
            if (_userEmail != value)
            {
                _userEmail = value;
                OnPropertyChanged(nameof(UserEmail));
            }
        }
    }

    private int _userSelectedSecretQuestionId;
    public int UserSelectedSecretQuestionId
    {
        get => _userSelectedSecretQuestionId;
        set
        {
            if(_userSelectedSecretQuestionId != value)
            {
                _userSelectedSecretQuestionId = value;
                OnPropertyChanged(nameof(UserSelectedSecretQuestionId));
            }
        }
    }

    private string _userSecretAnswer;
    public string UserSecretAnswer
    {
        get => _userSecretAnswer;
        set
        {
            if (_userSecretAnswer != value)
            {
                _userSecretAnswer = value;
                OnPropertyChanged(nameof(UserSecretAnswer));
            }
        }
    }

    public RegistrationViewModel(IConfiguration configuration)
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingUser>());
        _mapper = config.CreateMapper();
        this.configuration = configuration;
        RegistrationUserCommand = new GenericCommand<object>(RegistrationUser);
    }

    private async void RegistrationUser(object obj)
    {
        HubConnection connection = App.MainHubConnectionConfiguration();

        SignalRService signal = new SignalRService(connection);
        await signal.Connect();
        await signal.SendRegistrationMessage(UserLogin, UserPassword,UserEmail, UserSelectedSecretQuestionId, UserSecretAnswer);
        signal.ReceiveRegistrationMessage();
        Stopwatch stopwatch = Stopwatch.StartNew();
        while (true)
        {
            if (signal.IsRegistered != null || stopwatch.Elapsed.TotalSeconds >= 10) break;
            Task.Delay(100).Wait();
        }
        stopwatch.Stop();

        if (signal.IsRegistered == null) MessageBox.Show("Превышено время ожидания");
        else if (signal.IsRegistered == true) MessageBox.Show("Пользователь зарегистрирован!");
        else MessageBox.Show("Ошибка");
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}