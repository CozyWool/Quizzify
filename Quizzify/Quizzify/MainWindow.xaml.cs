using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Quizzify.DataAssecc.Contexts;
using System.IO;
using System.Windows;

namespace Quizzify;

public partial class MainWindow : Window
{
    private readonly IConfiguration _configuration;
    public MainWindow()
    {
        InitializeComponent();
        _configuration = GetConnectionString();
        var db = new DbQuizzifyContext(_configuration);
        db.Database.Migrate();  // миграция
    }
    private IConfiguration GetConnectionString()
    {
        var builder = new ConfigurationBuilder();
        // установка пути к текущему каталогу
        builder.SetBasePath(Directory.GetCurrentDirectory());
        // получаем конфигурацию из файла appsettings.json
        builder.AddJsonFile("appsettings.json");
        // создаем конфигурацию
        return builder.Build();
    }

}