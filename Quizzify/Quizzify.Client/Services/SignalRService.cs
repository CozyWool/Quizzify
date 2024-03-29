using Microsoft.AspNetCore.SignalR.Client;

namespace Quizzify.Client.Services
{
    public class SignalRService
    {
        private readonly HubConnection connection;

        private bool? isAutorized = null!;
        public bool? IsAutorized => isAutorized;

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

        public void ReceiveAutorizeMessage()
        {
            connection.On<bool>("ReceiveAutorize", (isAuthorized) =>
            {
                this.isAutorized = isAuthorized;
            });
        }

        public void ReceiveRegistrationMessage()
        {
            connection.On<bool>("ReceiveRegistration", (isRegistered) =>
            {
                this.isRegistered = isRegistered;
            });
        }

        public async Task SendAutorizeMessage(string name, string password)
        {
            await connection.SendAsync("SendAutorize", name, password);
        }

        public async Task SendRegistrationMessage(string userLogin, string userPassword, string userEmail, int userSelectedSecretQuestionId, string userSecretAnswer)
        {
            await connection.SendAsync("SendRegistration", userLogin, userPassword, userEmail, userSelectedSecretQuestionId, userSecretAnswer);
        }
    }
}
