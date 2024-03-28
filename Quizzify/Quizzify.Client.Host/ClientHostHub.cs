using Microsoft.AspNetCore.SignalR;
using Quizzify.DataAccess.Entities;

public class ClientHostHub : Hub
{
    private string _masterConnectionId;
    public List<PlayerEntity> Players { get; set; }
    public PackageEntity SelectedPackage { get; set; }
    private SessionState _sessionState;

    public ClientHostHub()
    {
        Players = new List<PlayerEntity>();
        SelectedPackage = new PackageEntity();
        _sessionState = SessionState.InLobby;
    }

    public Task RecievePackage(PackageEntity package)
    {
        if (Context.ConnectionId != _masterConnectionId) return Task.CompletedTask;

        SelectedPackage = package;
        return Task.CompletedTask;
    }

    public async Task RecievePlayer(PlayerEntity player)
    {
        if (Players.Contains(player)) return;

        Players.Add(player);
        if (Players.Count == 0)
            _masterConnectionId = Context.ConnectionId;
        await Clients.Others.SendAsync("RecieveNewPlayer", player);
    }

    public async Task StartGame()
    {
        if (Players.Count < 2) return;

        await Clients.Others.SendAsync("RecievePackage", SelectedPackage, Players, _sessionState);
        _sessionState = SessionState.InGame;
    }

    public override async Task OnConnectedAsync()
    {
        var context = Context.GetHttpContext();
        if (context != null)
            Console.WriteLine(
                $"[JOIN] {context.Connection.RemoteIpAddress?.ToString()} joined with connection id: {Context.ConnectionId}");

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? ex)
    {
        var context = Context.GetHttpContext();
        if (context != null)
            Console.WriteLine(
                $"[LEAVE] {context.Connection.RemoteIpAddress?.ToString()} left with connection id: {Context.ConnectionId}");
        await base.OnDisconnectedAsync(ex);
    }
}

public enum SessionState
{
    InLobby,
    InGame,
    ChoosePlayer,
    ChooseQuestion,
    WaitingAnswers,
    CheckAnswer,
    ShowingCorrectAnswer,
}