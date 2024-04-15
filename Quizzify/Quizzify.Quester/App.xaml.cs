using System.Windows;
using Microsoft.Extensions.Configuration;
using Quizzify.Quester.View;
using Quizzify.Quester.ViewModel;

namespace Quizzify.Quester;

public partial class App : Application
{
    private IConfiguration _configuration;
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        var questerViewModel = new QuesterViewModel(_configuration);
        var mainView = new MainView(questerViewModel);
        mainView.Show();

    }
    private IConfiguration BuildConfiguration()
    {
        return new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json").Build();
    }
}