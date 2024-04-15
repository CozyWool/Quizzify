using Microsoft.AspNetCore.SignalR.Client;
using Quizzify.Client.Model.Users;
using Quizzify.Client.View;
using Quizzify.Infrastructure.WPF.Command;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Quizzify.Client.ViewModel;
public class GameViewModel : INotifyPropertyChanged
{
    private PlayerModel player;
    private HubConnection _connection;

    private string _ipTextBox;
    public string IpTextBox
    {
        get => _ipTextBox;
        set
        {
            if (_ipTextBox == null) return;

            _ipTextBox = value;
            OnPropertyChanged(nameof(IpTextBox));
        }
    }

    private string _portTextBox;
    public string PortTextBox
    {
        get => _portTextBox;
        set
        {
            if (_portTextBox == null) return;

            _portTextBox = value;
            OnPropertyChanged(nameof(PortTextBox));
        }
    }

    private ICommand ConnectToGameLobbyCommand { get; }

    public GameViewModel() {}
    public GameViewModel(PlayerModel playerModel)
    {
        this.player = playerModel;
        ConnectToGameLobbyCommand = new GenericCommand<PlayerModel>(async (model) => await ConnectToGameLobby(model));
    }

    private async Task ConnectToGameLobby(PlayerModel model)
    {
        _connection = new HubConnectionBuilder()
            .WithUrl($"http://{IpTextBox}:{PortTextBox}/lobby")
            .Build();

        try
        {
            await _connection.StartAsync();
            MessageBox.Show("Успех");
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }

        var viewModel = new GameLobbyViewModel(player);
        var view = new GameLobbyView(viewModel);
        view.Show();
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}