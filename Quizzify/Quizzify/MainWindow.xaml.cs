using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Windows;
using Quizzify.DataAccess.Contexts;

namespace Quizzify;

public partial class MainWindow : Window
{
    private readonly IConfiguration _configuration;
    public MainWindow()
    {
        InitializeComponent();
        _configuration = GetConnectionString();
        var db = new DbQuizzifyContext(_configuration);
        db.Database.Migrate();
    }

    private IConfiguration GetConnectionString()
    {
        var builder = new ConfigurationBuilder();
        builder.SetBasePath(Directory.GetCurrentDirectory());
        builder.AddJsonFile("appsettings.json");
        return builder.Build();
    }

}