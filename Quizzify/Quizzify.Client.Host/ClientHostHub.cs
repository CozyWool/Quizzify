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
    private List<PlayerModel> Players { get; set; }
    private PackageModel SelectedPackage { get; set; }
    private int CurrentRoundIndex { get; set; }
    private SessionState _sessionState;
    private bool _isPlayerAnswering;
    private QuestionModel _selectedQuestion;

    public ClientHostHub()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingPlayer>());
        _mapper = config.CreateMapper();
        Players = new List<PlayerModel>();
        SelectedPackage = new PackageModel();
        CurrentRoundIndex = -1;
        _sessionState = SessionState.InLobby;
    }

    public Task ReceivePackage(PackageEntity package)
    {
        if (_sessionState != SessionState.InLobby) throw new Exception("Неподходящее состояние сессии!");
        if (Context.ConnectionId != _masterConnectionId)
            throw new Exception("Только ведущий может менять пакет с вопросами!");

        SelectedPackage = _mapper.Map<PackageModel>(package);
        return Task.CompletedTask;
    }

    public async Task ReceivePlayer(PlayerEntity player)
    {
        if (_sessionState != SessionState.InLobby) throw new Exception("Неподходящее состояние сессии!");

        var playerModel = _mapper.Map<PlayerModel>(player);
        playerModel.ConnectionId = Context.ConnectionId;
        if (Players.Contains(playerModel)) throw new Exception("Такой игрок уже существует!");

        Players.Add(playerModel);
        if (Players.Count == 0)
            _masterConnectionId = playerModel.ConnectionId;
        await SendPlayerInfo(playerModel);
    }

    public async Task ReceiveSessionState(SessionState sessionState)
    {
        if (Context.ConnectionId != _masterConnectionId)
            throw new Exception("Только ведущий может менять состояние сессии!");

        _sessionState = sessionState;
        await Clients.All.SendAsync("ReceiveSessionState", sessionState);
    }

    public async Task ReceiveSelectedPlayer(string connectionId)
    {
        if (_sessionState != SessionState.ChoosingPlayer) throw new Exception("Неподходящее состояние сессии!");

        if (Context.ConnectionId != _masterConnectionId)
            throw new Exception("Только ведущий может менять состояние сессии!");

        var playerModel = Players.Find(p => p.ConnectionId == connectionId) ??
                          throw new Exception($"Игрок с connectionId {connectionId} не найден!");
        _selectingPlayerConnectionId = playerModel.ConnectionId;
        await Clients.All.SendAsync("ReceiveSelectedPlayer", playerModel);
    }

    public async Task ReceiveSelectedQuestion(int themeId, int questionId)
    {
        if (_sessionState != SessionState.ChoosingQuestion) throw new Exception("Неподходящее состояние сессии!");

        if (Context.ConnectionId != _selectingPlayerConnectionId)
            throw new Exception("Только выбранный игрок может выбирать вопрос!");

        var selectedQuestion = SelectedPackage.Rounds[CurrentRoundIndex]
            .Themes.FirstOrDefault(p => p.ThemeId == themeId)
            ?.Questions.FirstOrDefault(p => p.QuestionId == questionId);

        _selectedQuestion = selectedQuestion ?? throw new Exception($"Вопрос с questionId {questionId} не найден!");
        var questionText = _selectedQuestion.QuestionText;

        await Clients.All.SendAsync("ReceiveSelectedQuestionText", questionText);
    }

    public async Task ReceiveAnsweringPlayer()
    {
        if (_sessionState != SessionState.WaitingAnswers) throw new Exception("Неподходящее состояние сессии!");

        if (_isPlayerAnswering) return;

        var connectionId = Context.ConnectionId;
        var playerModel = Players.Find(model => model.ConnectionId == connectionId);
        if (playerModel == null) throw new NullReferenceException("Игрок был null");

        playerModel.IsAnswering = true;
        _isPlayerAnswering = true;
        await Clients.All.SendAsync("ReceiveAnsweringPlayer", playerModel.ConnectionId);
    }

    public async Task ReceiveMasterVerdict(bool isAnswerCorrect)
    {
        if (_sessionState != SessionState.WaitingMasterVerdict) throw new Exception("Неподходящее состояние сессии!");

        if (Context.ConnectionId != _masterConnectionId)
            throw new Exception("Только ведущий может выносить вердикт!");

        var playerModel = Players.FirstOrDefault(model => model.IsAnswering);
        if (playerModel == null) throw new NullReferenceException("Игрок был null");

        if (isAnswerCorrect)
        {
            playerModel.Points += _selectedQuestion.QuestionCost;
        }
        else
        {
            playerModel.Points -= -_selectedQuestion.QuestionCost;
        }

        playerModel.IsAnswering = false;
        await Clients.All.SendAsync("RecieveAnswerResult", isAnswerCorrect, playerModel);
    }

    public async Task RecieveCorrectAnswer()
    {
        if (_sessionState != SessionState.ShowingCorrectAnswer) throw new Exception("Неподходящее состояние сессии!");

        if (Context.ConnectionId != _masterConnectionId)
            throw new Exception("Только ведущий может выносить вердикт!");

        var answerModel = _mapper.Map<AnswerModel>(_selectedQuestion) ??
                          throw new NullReferenceException("Ответ был null");
        await Clients.All.SendAsync("ReceiveCorrectAnswer", answerModel, _selectedQuestion);
        SelectedPackage
            .Rounds[CurrentRoundIndex]
            .Themes.FirstOrDefault(model => model.ThemeId == _selectedQuestion.ThemeId)
            ?.Questions.Remove(_selectedQuestion);
        var isRoundEnded = SelectedPackage.Rounds[CurrentRoundIndex].Themes.Select(themes => themes.Questions.Count > 0)
            .Contains(true);
        if (!isRoundEnded) return;

        if (CurrentRoundIndex + 1 < SelectedPackage.Rounds.Count)
        {
            CurrentRoundIndex++;
            await Clients.All.SendAsync("RecieveSwitchRound", _sessionState, CurrentRoundIndex);
            return;
        }
        
        await EndGame();
    }

    private async Task SendPlayerInfo(PlayerModel player)
    {
        await Clients.All.SendAsync("ReceivePlayerInfo", player);
    }

    public async Task SendPlayersInfo()
    {
        foreach (var player in Players)
        {
            await Clients.Caller.SendAsync("ReceivePlayerInfo", player);
        }
    }

    public async Task SendError(string errorMessage)
    {
        await Clients.All.SendAsync("ReceiveError", errorMessage);
    }

    public async Task StartGame()
    {
        if (Players.Count < 2) throw new Exception("Недостаточно игроков! Необходимо минимум 2 игрока.");
        CurrentRoundIndex = 0;
        await Clients.All.SendAsync("ReceivePackage", SelectedPackage, Players, _sessionState);
    }

    private async Task EndGame()
    {
        _sessionState = SessionState.GameEndingScreen;
        var winner = Players.OrderBy(model => model.Points).Last();
        await Clients.All.SendAsync("RecieveWinner", winner, _sessionState);
    }

    public override async Task OnConnectedAsync()
    {
        var context = Context.GetHttpContext();
        if (context != null)
            Console.WriteLine(
                $"[JOIN] {context.Connection.RemoteIpAddress?.ToString()} joined with connection id: {Context.ConnectionId}");
        await base.OnConnectedAsync();
    }

    //TODO: Доделать отключение
    public override async Task OnDisconnectedAsync(Exception? ex)
    {
        var context = Context.GetHttpContext();
        if (context != null)
            Console.WriteLine(
                $"[LEAVE] {context.Connection.RemoteIpAddress?.ToString()} left with connection id: {Context.ConnectionId}");
        await base.OnDisconnectedAsync(ex);
    }
}