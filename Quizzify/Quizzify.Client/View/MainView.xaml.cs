using System.IO;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Quizzify.Client.ViewModel;
using Quizzify.DataAccess.Contexts;

namespace Quizzify.Client.View;

public partial class MainView : Window
{
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