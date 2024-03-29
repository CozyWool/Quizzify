using Microsoft.AspNetCore.SignalR;
using Quizzify.DataAccess.Contexts;

namespace Quizzify.MainServer.Hubs
{
    public class MainHub:Hub
    {
        //private DatabaseManager databaseManager;

        //public MainHub()
        //{
        //    //databaseManager = new DatabaseManager();
        //}

        public async Task AutorizeSend(string name, string password)
        {
            await Clients.Caller.SendAsync("ReceiveAutorize", LoginVerification(name, password));
        }

        public async Task RegistrationSend(string name, string password)
        {
            await Clients.Caller.SendAsync("ReceiveRegistration", RegistrationVerification(name, password));
        }

        private bool LoginVerification(string name, string password)
        {
            //взаимодействие с бд
            return true;
        }

        private bool RegistrationVerification(string name, string password)
        {
            var userEntity=ConvertTo
            using var context = new DbQuizzifyContext(configuration);
            context.AddUser(userEntity);
        }
    }
}
