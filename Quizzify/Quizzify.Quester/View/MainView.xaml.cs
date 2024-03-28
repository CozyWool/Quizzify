using System.Windows;
using Quizzify.Quester.ViewModel;

namespace Quizzify.Quester.View;

public partial class MainView : Window
{
    public MainView(MainViewModel mainViewModel)
    {
        InitializeComponent();
        DataContext = mainViewModel;
    }
}