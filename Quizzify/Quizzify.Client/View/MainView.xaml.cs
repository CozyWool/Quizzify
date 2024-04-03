using System.Windows;
using Microsoft.AspNetCore.SignalR.Client;
using Quizzify.Client.ViewModel;

namespace Quizzify.Client.View;

public partial class MainView : Window
{
    private HubConnection _connection;

    public MainView(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    // TODO: Выпилить в класс модели
    // private static IConfiguration GetConnectionString()
    // {
    //     var builder = new ConfigurationBuilder();
    //     builder.SetBasePath(Directory.GetCurrentDirectory());
    //     builder.AddJsonFile("appsettings.json");
    //     return builder.Build();
    // }
}