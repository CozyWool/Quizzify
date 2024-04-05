using System.Windows;
using Quizzify.Quester.View;
using Quizzify.Quester.ViewModel;

namespace Quizzify.Quester;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        var questerViewModel = new QuesterViewModel();
        var mainView = new MainView(questerViewModel);
        mainView.Show();
    }
}