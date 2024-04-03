using Microsoft.AspNetCore.SignalR;
using Quizzify.DataAccess.Contexts;
using Quizzify.DataAccess.Entities;
using Quizzify.MainServer.Models.Users;
using AutoMapper;
using Quizzify.MainServer.Mappers;
using System.Runtime.Intrinsics.Arm;
using Quizzify.Infrastructure.Security;

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

        public async Task SendAuthorize(AuthorizationModel user)
        {
            await Clients.Caller.SendAsync("ReceiveAuthorize", LoginVerification(user));
        }

        public async Task SendRegistration(RegistrationModel newUser)
        {
            await Clients.Caller.SendAsync("ReceiveRegistration", RegistrationVerification(newUser));
        }

        private bool LoginVerification(AuthorizationModel user)
        {
            var context = new DbQuizzifyContext(configuration);
            foreach (var userItem in context.Users)
            {
                if (userItem.Email==user.LoginOrEmail)
                {
                    var aes = new AESManager();
                    var decryptedPassword = aes.Decrypt(userItem.PasswordHash);
                    if (decryptedPassword == user.Password)
                    {
                        return true;
                    }
                    break;
                }
            }
            return false;
        }

        private bool RegistrationVerification(RegistrationModel newUser)
        {
            var userEntity = _mapper.Map<UserEntity>(newUser);
            using var context = new DbQuizzifyContext(configuration);
            context.AddUser(userEntity);
            return true;
        }
    }
}
