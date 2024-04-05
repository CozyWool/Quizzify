using System.Windows;
using Quizzify.Quester.ViewModel;

namespace Quizzify.Quester.View;

public partial class MainView : Window
{
    public MainView(QuesterViewModel questerViewModel)
    {
        InitializeComponent();
        DataContext = questerViewModel;
    }
}