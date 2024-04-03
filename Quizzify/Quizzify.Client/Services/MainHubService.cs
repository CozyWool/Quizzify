using Microsoft.AspNetCore.SignalR.Client;
using Quizzify.Client.Model.Users;

namespace Quizzify.Client.Services
{
    public class MainHubService
    {
        public event Action<bool?> AuthorizationResponseArrived;
        public event Action<bool?> RegistartionResponseArrived;
        private readonly HubConnection connection;

        public MainHubService(HubConnection connection)
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
                AuthorizationResponseArrived?.Invoke(isAuthorized);
            });
        }

        public void ReceiveRegistrationMessage()
        {
            connection.On<bool>("ReceiveRegistration", (isRegistered) =>
            {
                RegistartionResponseArrived.Invoke(isRegistered);
            });
        }

        public async Task SendAuthorizeMessage(AuthorizationModel user)
        {
            await connection.SendAsync("SendAuthorize", user);
        }

        public async Task SendRegistrationMessage(RegistrationModel newUser)
        {
            await connection.SendAsync("SendRegistration", newUser);
        }
    }
}
