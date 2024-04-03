using Microsoft.AspNetCore.SignalR.Client;
using Quizzify.Client.Model.Users;

namespace Quizzify.Client.Services
{
    public class MainHubService
    {
        public event Action<bool?> AuthorizationResponseArrived;
        public event Action<bool?> RegistartionResponseArrived;
        private readonly HubConnection _connection;

        public MainHubService(HubConnection connection)
        {
            this._connection = connection;
        }

        public async Task Connect()
        {
            await _connection.StartAsync();
        }

        public void ReceiveAuthorizeMessage()
        {
            _connection.On<bool>("ReceiveAuthorize", (isAuthorized) =>
            {
                AuthorizationResponseArrived?.Invoke(isAuthorized);
            });
        }

        public void ReceiveRegistrationMessage()
        {
            _connection.On<bool>("ReceiveRegistration", (isRegistered) =>
            {
                RegistartionResponseArrived.Invoke(isRegistered);
            });
        }

        public async Task SendAuthorizeMessage(AuthorizationModel user)
        {
            await _connection.SendAsync("SendAuthorize", user);
        }

        public async Task SendRegistrationMessage(RegistrationModel newUser)
        {
            await _connection.SendAsync("SendRegistration", newUser);
        }
    }
}
