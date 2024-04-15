using Quizzify.Client.ViewModel;
using System.Windows;

namespace Quizzify.Client.View;

public partial class GameView : Window
{
    public GameView(GameViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
