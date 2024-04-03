using Newtonsoft.Json;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows;
using Quizzify.Client.Settings.Language;
using Quizzify.Client.View;
using Quizzify.Client.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.SignalR.Client;

namespace Quizzify.Client;

public partial class App : Application
{
    private IConfiguration _configuration;

    private const string AppDataFolder = "AppData";
    private const string LocalFolder = "Local";
    private const string SettingsFile = "appsettings.json";
    private const string LocalesFolder = "Locales";

    public static HubConnection HubConnectionConfiguration(string ip, int port, string route)
    {
        return new HubConnectionBuilder()
        .WithUrl($"http://{ip}:{port}/{route}")
        .Build();
    }

    public static HubConnection MainHubConnectionConfiguration()
    {
        return HubConnectionConfiguration("localhost", 5234, "mainhub");   
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        ApplyCultureFromSettingsFile();
        _configuration = BuildConfiguration();
        base.OnStartup(e);
        
        var registrationViewModel = new RegistrationViewModel(_configuration);
        var registrationView = new RegistrationView(registrationViewModel);
        registrationView.Show();
    }

    private IConfiguration BuildConfiguration()
    {
        return new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json").Build();
    }

    private void ApplyCultureFromSettingsFile()
    {
        var filePath = GetLocalizationFilePath();
        if (!string.IsNullOrEmpty(filePath))
        {
            var mainSettings = LoadMainSettings(filePath);
            if (MainSettings.IsNullOrEmpty(mainSettings)) return;
            
            var languageCode = mainSettings?.LanguageSettings.LanguageCode;
            if (!string.IsNullOrEmpty(languageCode))
            {
                SetApplicationLanguage(languageCode);
                return;
            }
            ShowMessage("В файле локализации нет информации о языке");
        }
        else ShowMessage("Файл локализации не найден.");
    }


    private string GetLocalizationFilePath()
    {
        var userProfileDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var projectName = Assembly.GetExecutingAssembly().GetName().Name;
        var localizationFolderPath = Path.Combine(userProfileDirectory, AppDataFolder, LocalFolder, projectName, LocalesFolder);
        var filePath = Path.Combine(localizationFolderPath, SettingsFile);


        if (!Directory.Exists(localizationFolderPath))
        {
            Directory.CreateDirectory(localizationFolderPath);
        }

        if (File.Exists(filePath)) return File.Exists(filePath) ? filePath : null;
        
        var settings = new MainSettings
        {
            LanguageSettings = new LanguageSettings { LanguageCode = "ru-RU" }
        };

        var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
        File.WriteAllText(filePath, json);

        return File.Exists(filePath) ? filePath : null;
    }

    private MainSettings LoadMainSettings(string filePath)
    {
        try
        {
            using var reader = new StreamReader(filePath);
            var jsonData = reader.ReadToEnd();

            if (!string.IsNullOrEmpty(jsonData))
            {
                return JsonConvert.DeserializeObject<MainSettings>(jsonData);
            }

            ShowMessage(HandleException(new Exception(), "Файл настроек локализации пуст"));
            return null;
        }
        catch(Exception ex)
        {
            ShowMessage(HandleException(ex, "Произошла ошибка при загрузке файла с настройками: "));
            return null;
        }
    }

    private void SetApplicationLanguage(string languageCode)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo(languageCode);
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(languageCode);
    }

    private void ShowMessage(string message) => MessageBox.Show(message);

    private string HandleException(Exception ex, string message) => $"{message} {ex.Message}";
}