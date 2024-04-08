using Microsoft.AspNetCore.SignalR.Client;
using Quizzify.Client.Enums;
using Quizzify.Client.Model;
using Quizzify.Client.Model.Users;
using System.ComponentModel;
using System.Windows.Controls;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Quizzify.Client.Services;

public class ClientHubService : INotifyPropertyChanged
{
    public event Action<PlayerModel> InfoNewPlayerArrived;
    public event Action<string> ErrorArrived;
   

    private readonly HubConnection _connection;

    private PackageModel _packageModel;
    private PlayerModel _currentPlayer;

    private ContentControl _content;
    public ContentControl Content 
    
    {
        get => _content;
        set
        {
            if (_content == null) return;
            _content = value;
            OnPropertyChanged(nameof(Content));
        }
    }
    

    public ClientHubService(HubConnection connection)
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

    public async Task ReceivePackage()
    {
        _connection.On<PackageModel, PlayerModel, SessionState>("ReceivePackage", (packageModel, playerModel, sessionState) =>
        {
            if (sessionState == SessionState.InLobby)
            {
                _packageModel = packageModel;
                _currentPlayer = playerModel;
            }
            Content.Content = _packageModel.Rounds[0];
        });
    }

    public async Task ReceiveSelectedQuestionText()
    {
        _connection.On<QuestionModel>("ReceiveSelectedQuestionText", (questionModel) =>
        {
            string questionText = questionModel.QuestionText;
            Content.Content = questionText;
        });
    }

    public async Task SendSelectedQuestionText(int themeId, int questionId)
    {
        await _connection.SendAsync("ReceiveSelectedQuestionThemeAndQuestionId", themeId, questionId);
    }

    public async Task ReceiveAffirmationKey()
    {
        
        await _connection.SendAsync("ReceiveAnsweringPlayer", _currentPlayer.ConnectionId);
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
