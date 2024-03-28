using Microsoft.Extensions.Configuration;
using Quizzify.Client.Model.Users;
using Quizzify.DataAccess.Contexts;
using Quizzify.DataAccess.Entities;
using System.ComponentModel;
using System.Windows.Input;

namespace Quizzify.Client.ViewModel;

	public class RegistrationViewModel : INotifyPropertyChanged
{
    private readonly IConfiguration configuration;
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

    public RegistrationViewModel()
    {
        configuration = BuildConfiguration();
        RegistrationUserCommand = new DelegateCommand(RegistrationUser, _ => true);
    }

    private IConfiguration BuildConfiguration()
    {
        return new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json").Build();
    }

    private void RegistrationUser(object obj)
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
        
        var userEntity = ConvertToUserEntity(newUser);
        using var context = new DbQuizzifyContext(configuration);
        context.AddUser(userEntity);
    }

    private UserEntity ConvertToUserEntity(RegistrationModel registrationModel)
    {
        return new UserEntity
        {
            UserId = registrationModel.UserId,
            Login = registrationModel.Login,
            PasswordHash = registrationModel.Password,
            Email = registrationModel.Email,
            SelectedSecretQuestionId = registrationModel.SelectedSecretQuestionId,
            SecretAnswerHash = registrationModel.SecretAnswer
        };
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
//генерируется