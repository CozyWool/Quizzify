using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Quizzify.Client.Host.Enums;
using Quizzify.Client.Host.Mappers;
using Quizzify.Client.Host.Model;
using Quizzify.DataAccess.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Quizzify.Client.Host;

public class ClientHostHub : Hub
{
    private string _masterConnectionId;
    private string _selectingPlayerConnectionId;
    private readonly IMapper _mapper;
    public List<PlayerModel> Players { get; set; }
    public PackageModel SelectedPackage { get; set; }
    public int CurrentRoundIndex { get; set; }
    private SessionState _sessionState;

    public ClientHostHub()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingPlayer>());
        _mapper = config.CreateMapper();
        Players = new List<PlayerModel>();
        SelectedPackage = new PackageModel();
        CurrentRoundIndex = -1;
        _sessionState = SessionState.InLobby;
    }

    //TODO:Подумать оставлять ли async
    public async Task RecievePackage(PackageEntity package)
    {
        if (Context.ConnectionId != _masterConnectionId)
            throw new Exception("Только ведущий может менять пакет с вопросами!");
        //TODO:Подумать оставлять ли условие
        if (package.Rounds.Count == 0) throw new Exception("В пакете не может быть 0 раундов!");

        SelectedPackage = _mapper.Map<PackageModel>(package);
    }

    public async Task RecievePlayer(PlayerEntity player)
    {
        var playerModel = _mapper.Map<PlayerModel>(player);
        playerModel.ConnectionId = Context.ConnectionId;
        if (Players.Contains(playerModel)) throw new Exception("Такой игрок уже существует!");

        Players.Add(playerModel);
        if (Players.Count == 0)
            _masterConnectionId = playerModel.ConnectionId;
        await SendPlayerInfo(playerModel);
    }

    public async Task RecieveSessionState(SessionState sessionState)
    {
        if (Context.ConnectionId != _masterConnectionId)
            throw new Exception("Только ведущий может менять состояние сессии!");

        _sessionState = sessionState;
        await Clients.All.SendAsync("RecieveSessionState", sessionState);
    }

    public async Task RecieveSelectedPlayer(string connectionId)
    {
        if (Context.ConnectionId != _masterConnectionId)
            throw new Exception("Только ведущий может менять состояние сессии!");

        var playerModel = Players.Find(p => p.ConnectionId == connectionId) ??
                          throw new Exception($"Игрок с connectionId {connectionId} не найден!");
        _selectingPlayerConnectionId = playerModel.ConnectionId;
        await Clients.All.SendAsync("RecieveSelectedPlayer", playerModel);
    }

    public async Task RecieveSelectedQuestion(int roundId, int themeId, int questionId)
    {
        if (Context.ConnectionId != _selectingPlayerConnectionId)
            throw new Exception("Только выбранный игрок может выбирать вопрос!");

        var selectedQuestion = SelectedPackage.Rounds.FirstOrDefault(p => p.RoundId == roundId).Themes.FirstOrDefault(p => p.ThemeId == themeId).Questions.FirstOrDefault(p => p.QuestionId == questionId);

        if(selectedQuestion == null)
            throw new Exception($"Вопрос с questionId {questionId} не найден!");

        string questionText = selectedQuestion.QuestionText;

        await Clients.All.SendAsync("RecieveSelectedQuestionText", questionText);
    }

    public async Task SendPlayersInfo()
    {
        for (int i = 0; i < Players.Count; i++)
        {
            if (Players[i] != null) continue;
            await Clients.Caller.SendAsync("ReceivePlayerInfo", Players[i]);
        }
    }

    public async Task StartGame()
    {
        if (Players.Count < 2) throw new Exception("Недостаточно игроков! Необходимо минимум 2 игрока.");
        CurrentRoundIndex = 0;
        //TODO: Передавать номер раунда и сделать переключение раундов в целом
        await Clients.All.SendAsync("RecievePackage", SelectedPackage, Players, _sessionState);
        _sessionState = SessionState.InGame;
    }

    //TODO: Сделать функцию получения всех игроков
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

    private async Task SendPlayerInfo(PlayerModel player)
    {
        if (player != null) return;
        await Clients.All.SendAsync("ReceivePlayerInfo", player);
    }
}