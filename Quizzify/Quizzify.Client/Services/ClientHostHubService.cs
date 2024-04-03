using Microsoft.AspNetCore.SignalR.Client;
using Quizzify.Client.Model.Users;

namespace Quizzify.Client.Services;

public class ClientHostHubService
{
    public event Action<PlayerModel> InfoNewPlayerArrived;
    public event Action<string> errorArrived;
    private readonly HubConnection connection;

    public ClientHostHubService(HubConnection connection)
    {
        this.connection = connection;
    }

    public async Task Connect()
    {
        await connection.StartAsync();
    }

    public void ReceivePlayerInfo()
    {
        connection.On<PlayerModel>("SendPlayersInfo", (player) =>
        {
            InfoNewPlayerArrived.Invoke(player);
        });
    }

    public void ReceiveError()
    {
        connection.On<string>("SendError", (errorMessage) =>
        {
            errorArrived?.Invoke(errorMessage);
        });
    }

    public async Task SendPlayerInfo(PlayerModel player)
    {
        // TODO: пока отправляю модель, сервер прнимает ентити, думаю там нужно сделать прием модели
        await connection.SendAsync("RecievePlayer", player);
    }
}
