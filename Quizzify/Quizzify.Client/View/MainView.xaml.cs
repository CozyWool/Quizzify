using System.IO;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Quizzify.DataAccess.Contexts;

namespace Quizzify.Client.View;

public partial class MainView : Window
{
    private readonly IConfiguration _configuration;
    public MainView()
    {
        InitializeComponent();
        _configuration = GetConnectionString();
        var db = new DbQuizzifyContext(_configuration);
        // db.Database.Migrate();
    }

    private static IConfiguration GetConnectionString()
    {
        var builder = new ConfigurationBuilder();
        builder.SetBasePath(Directory.GetCurrentDirectory());
        builder.AddJsonFile("appsettings.json");
        return builder.Build();
    }

}