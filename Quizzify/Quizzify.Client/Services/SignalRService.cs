using Microsoft.AspNetCore.SignalR.Client;
using Quizzify.Client.Model.Users;

namespace Quizzify.Client.Services
{
    public class SignalRService
    {
        private readonly HubConnection connection;

        private bool? isAuthorized = null!;
        public bool? IsAuthorized => isAuthorized;

        private bool? isRegistered = null!;
        public bool? IsRegistered => isRegistered;

        public SignalRService(HubConnection connection)
        {
            this.connection = connection;
        }

        public async Task Connect()
        {
            await connection.StartAsync();
        }

        public void ReceiveAuthorizeMessage()
        {
            connection.On<bool>("ReceiveAuthorize", (isAuthorized) =>
            {
                this.isAuthorized = isAuthorized;
            });
        }

        public void ReceiveRegistrationMessage()
        {
            connection.On<bool>("ReceiveRegistration", (isRegistered) =>
            {
                this.isRegistered = isRegistered;
            });
        }

        public async Task SendAuthorizeMessage(string userLoginOrEmail, string userPassword)
        {
            await connection.SendAsync("SendAuthorize", userLoginOrEmail, userPassword);
        }

        public async Task SendRegistrationMessage(RegistrationModel newUser)
        {
            await connection.SendAsync("SendRegistration", newUser);
        }
    }
}
