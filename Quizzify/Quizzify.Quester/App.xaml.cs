using System.Windows;
using Quizzify.Quester.View;
using Quizzify.Quester.ViewModel;

namespace Quizzify.Quester;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        var mainViewModel = new MainViewModel();
        var mainView = new MainView(mainViewModel);
        mainView.Show();
    }
}