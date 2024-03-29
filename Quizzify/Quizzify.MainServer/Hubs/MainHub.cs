using Microsoft.AspNetCore.SignalR;
using Quizzify.DataAccess.Contexts;
using Quizzify.DataAccess.Entities;
using Quizzify.MainServer.Models.Users;
using AutoMapper;
using Quizzify.MainServer.Mappers;

namespace Quizzify.MainServer.Hubs
{
    public class MainHub:Hub
    {
        private readonly IConfiguration configuration;
        private readonly IMapper _mapper;

        public MainHub()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingUser>());
            _mapper = config.CreateMapper();
            configuration= new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json").Build();
        }

        public async Task SendAuthorize(string userLoginOrEmail, string userPassword)
        {
            await Clients.Caller.SendAsync("ReceiveAuthorize", LoginVerification(userLoginOrEmail, userPassword));
        }

        public async Task SendRegistration(string userLogin, string userPassword, string userEmail, int userSelectedSecretQuestionId, string userSecretAnswer)
        {
            await Clients.Caller.SendAsync("ReceiveRegistration", RegistrationVerification(userLogin, userPassword, userEmail, userSelectedSecretQuestionId, userSecretAnswer));
        }

        private bool LoginVerification(string userLoginOrEmail, string userPassword)
        {
            var context = new DbQuizzifyContext(configuration);
            foreach (var user in context.Users)
            {
                if (user.Login == userLoginOrEmail || user.Email==userLoginOrEmail)
                {
                    if (user.PasswordHash == userPassword) return true;//Тут наверное надо как расшифровывать
                    break;
                }
            }
            return false;
        }

        private bool RegistrationVerification(string userLogin, string userPassword, string userEmail, int userSelectedSecretQuestionId, string userSecretAnswer)
        {
            var aes = new AESManager();
            Guid guid = Guid.NewGuid();

            byte[] saltBytes = guid.ToByteArray();
            string salt = Convert.ToBase64String(saltBytes);

            string encryptedPassword = aes.Encrypt(userPassword, salt);

            var newUser = new RegistrationModel()
            {
                UserId = guid,
                Login = userLogin,
                Password = encryptedPassword,
                Email = userEmail,
                SelectedSecretQuestionId = userSelectedSecretQuestionId,
                SecretAnswer = userSecretAnswer
            };

            var userEntity = _mapper.Map<UserEntity>(newUser);
            using var context = new DbQuizzifyContext(configuration);
            context.AddUser(userEntity);
            return true;
        }
    }
}
