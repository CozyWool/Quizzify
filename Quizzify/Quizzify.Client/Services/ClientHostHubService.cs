using Microsoft.AspNetCore.SignalR.Client;
using Quizzify.Client.Model.Users;

namespace Quizzify.Client.Services;

public class ClientHostHubService
{
    public event Action<PlayerModel> InfoNewPlayerArrived;
    public event Action<string> ErrorArrived;
    private readonly HubConnection _connection;

    public ClientHostHubService(HubConnection connection)
    {
        this._connection = connection;
    }

    public async Task Connect()
    {
        await _connection.StartAsync();
    }

    public void ReceivePlayerInfo()
    {
        _connection.On<PlayerModel>("SendPlayersInfo", (player) =>
        {
            InfoNewPlayerArrived.Invoke(player);
        });
    }

    public void ReceiveError()
    {
        _connection.On<string>("SendError", (errorMessage) =>
        {
            ErrorArrived?.Invoke(errorMessage);
        });
    }

    public async Task SendPlayerInfo(PlayerModel player)
    {
        // TODO: пока отправляю модель, сервер прнимает ентити, думаю там нужно сделать прием модели
        await _connection.SendAsync("RecievePlayer", player);
    }
}
