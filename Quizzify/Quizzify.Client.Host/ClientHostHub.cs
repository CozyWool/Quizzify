using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Quizzify.Client.Host.Enums;
using Quizzify.Client.Host.Mappers;
using Quizzify.Client.Host.Model;
using Quizzify.DataAccess.Entities;

namespace Quizzify.Client.Host;

public class ClientHostHub : Hub
{
    private string _masterConnectionId;
    private string _selectingPlayerConnectionId;
    private readonly IMapper _mapper;
    public List<PlayerModel> Players { get; set; }
    public PackageEntity SelectedPackage { get; set; }
    public int CurrentRoundIndex { get; set; }
    private SessionState _sessionState;

    public ClientHostHub()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingPlayer>());
        _mapper = config.CreateMapper();
        Players = new List<PlayerModel>();
        SelectedPackage = new PackageEntity();
        CurrentRoundIndex = -1;
        _sessionState = SessionState.InLobby;
    }

    public async Task RecievePackage(PackageEntity package)
    {
        if (Context.ConnectionId != _masterConnectionId) throw new Exception("Только ведущий может менять пакет с вопросами!");
        if (package.Rounds.Count == 0) throw new Exception("В пакете не может быть 0 раундов!");
        
        SelectedPackage = package;
    }

    public async Task RecievePlayer(PlayerEntity player)
    {
        var playerModel = _mapper.Map<PlayerModel>(player);
        playerModel.ConnectionId = Context.ConnectionId;
        if (Players.Contains(playerModel)) throw new Exception("Такой игрок уже существует!");

        Players.Add(playerModel);
        if (Players.Count == 0)
            _masterConnectionId = playerModel.ConnectionId;
        await Clients.Others.SendAsync("RecieveNewPlayer", playerModel);
    }

    public async Task RecieveSessionState(SessionState sessionState)
    {
        if (Context.ConnectionId != _masterConnectionId) throw new Exception("Только ведущий может менять состояние сессии!");
        
        _sessionState = sessionState;
        await Clients.All.SendAsync("RecieveSessionState", sessionState);
    }
    public async Task RecieveSelectedPlayer(string connectionId)
    {
        if (Context.ConnectionId != _masterConnectionId) throw new Exception("Только ведущий может менять состояние сессии!");
        
        var playerModel = Players.Find(p => p.ConnectionId == connectionId) ?? throw new Exception($"Игрок с connectionId {connectionId} не найден!");
        _selectingPlayerConnectionId = playerModel.ConnectionId;
        await Clients.All.SendAsync("RecieveSelectedPlayer", playerModel);
    }
    public async Task RecieveSelectedQuestion(QuestionEntity questionEntity)
    {
        //TODO:Доделать
        if (Context.ConnectionId != _selectingPlayerConnectionId) throw new Exception("Только выбранный игрок может выбирать вопрос!");
        if (SelectedPackage.Rounds)
        {
            
        }
        _selectingPlayerConnectionId = playerModel.ConnectionId;
        await Clients.All.SendAsync("RecieveSelectedPlayer", playerModel);
    }
    

    public async Task StartGame()
    {
        if (Players.Count < 2) throw new Exception("Недостаточно игроков! Необходимо минимум 2 игрока.");
        CurrentRoundIndex = 0;
        await Clients.All.SendAsync("RecievePackage", SelectedPackage, Players, _sessionState);
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