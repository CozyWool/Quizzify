using Quizzify.Client.Model.Users;
using System.Windows;

namespace Quizzify.Client.View;

public partial class GameLobbyView : Window
{
    public GameLobbyView(GameLobbyViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}