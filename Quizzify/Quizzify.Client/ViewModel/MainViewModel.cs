using Quizzify.Client.Command;
using Quizzify.Client.Model.Users;
using Quizzify.Client.Services;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Quizzify.Client.ViewModel;

public class MainViewModel : INotifyPropertyChanged
{
    public ICommand ConnectToGameCommand { get; }
    private List<PlayerModel> _players;
    private PlayerModel _player;

    private string _ip;
    public string IP
    {
        get => _ip;
        set
        {
            _ip = value;
            OnPropertyChanged(nameof(IP));
        }
    }

    private string _port;
    public string Port
    {
        get => _port;
        set
        {
            _port = value;
            OnPropertyChanged(nameof(Port));
        }
    }

    public MainViewModel(PlayerModel player)
    {
        this._player= player;
        ConnectToGameCommand = new GenericCommand<object>(ConnectToGame);
    }

    public async void ConnectToGame(object obj)
    {
        try
        {
            var connection = App.HubConnectionConfiguration("localhost", 5098, "host");
            var signal = new ClientHostHubService(connection);
            await signal.Connect();
            await signal.SendPlayerInfo(_player);
            signal.ReceivePlayerInfo();
            signal.InfoNewPlayerArrived += AddPlayer;
            //TODO --- обновление view
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void AddPlayer(PlayerModel playerModel)
    {
        if (playerModel == null) return;
        if (_players.Contains(playerModel)) return;
        _players.Add(playerModel);
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}