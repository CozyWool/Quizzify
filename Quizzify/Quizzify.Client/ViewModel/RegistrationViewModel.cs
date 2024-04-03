using AutoMapper;
using Microsoft.Extensions.Configuration;
using Quizzify.Client.Mappers;
using System.ComponentModel;
using System.Windows.Input;

using Microsoft.AspNetCore.SignalR.Client;
using System.Diagnostics;
using System.Windows;
using Quizzify.Client.Services;
using Quizzify.Client.Model.Users;
using Quizzify.Infrastructure.Security;
using Quizzify.Infrastructure.WPF.Command;

namespace Quizzify.Client.ViewModel;

public class RegistrationViewModel : INotifyPropertyChanged
{
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    private bool? _isRegistered = null!;
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
        this._configuration = configuration;
        RegistrationUserCommand = new GenericCommand<object>(RegistrationUser);
    }

    private void SetRegistration(bool? b)
    {
        _isRegistered = b;
    }

    private async void RegistrationUser(object obj)
    {
        var aes = new AESManager();
        Guid guid = Guid.NewGuid();

        byte[] saltBytes = guid.ToByteArray();
        string salt = Convert.ToBase64String(saltBytes);

        string encryptedPassword = aes.Encrypt(UserPassword, salt);

        var newUser = new RegistrationModel()
        {
            UserId = guid,
            Login = UserLogin,
            Password = encryptedPassword,
            Email = UserEmail,
            SelectedSecretQuestionId = UserSelectedSecretQuestionId,
            SecretAnswer = UserSecretAnswer
        };

        HubConnection connection = App.MainHubConnectionConfiguration();
        MainHubService signal = new MainHubService(connection);
        int waitingTime = 10;

        await signal.Connect();
        await signal.SendRegistrationMessage(newUser);
        signal.ReceiveRegistrationMessage();
        signal.RegistartionResponseArrived += SetRegistration;

        Stopwatch stopwatch = Stopwatch.StartNew();
        while (true)
        {
            if (_isRegistered != null || stopwatch.Elapsed.TotalSeconds >= waitingTime) break;
            Task.Delay(100).Wait();
        }
        stopwatch.Stop();

        if (_isRegistered == null) MessageBox.Show("Превышено время ожидания");
        else if (_isRegistered == true) MessageBox.Show("Пользователь зарегистрирован!");
        else MessageBox.Show("Ошибка");
        signal.RegistartionResponseArrived -= SetRegistration;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}