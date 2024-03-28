using Newtonsoft.Json;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows;
using Quizzify.Client.Settings.Language;
using Quizzify.Client.View;
using Quizzify.Client.ViewModel;

namespace Quizzify.Client;

public partial class App : Application
{
    private const string AppDataFolder = "AppData";
    private const string LocalFolder = "Local";
    private const string SettingsFile = "appsettings.json";
    private const string LocalesFolder = "Locales";

    protected override void OnStartup(StartupEventArgs e)
    {
        ApplyCultureFromSettingsFile();
        base.OnStartup(e);
        
        var registrationViewModel = new RegistrationViewModel();
        var registrationView = new RegistrationView(registrationViewModel);
        registrationView.Show();
    }

    private void ApplyCultureFromSettingsFile()
    {
        var filePath = GetLocalizationFilePath();
        if (!string.IsNullOrEmpty(filePath))
        {
            var mainSettings = LoadMainSettings(filePath);
            if (!MainSettings.IsNullOrEmpty(mainSettings))
            {
                var languageCode = mainSettings?.LanguageSettings.LanguageCode;
                if (!string.IsNullOrEmpty(languageCode))
                {
                    SetApplicationLanguage(languageCode);
                    return;
                }
                ShowMessage("В файле локализации нет информации о языке");
            }
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

        if (!File.Exists(filePath))
        {
            var settings = new MainSettings
            {
                LanguageSettings = new LanguageSettings { LanguageCode = "ru-RU" }
            };

            string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        return File.Exists(filePath) ? filePath : null;
    }

    private MainSettings LoadMainSettings(string filePath)
    {
        try
        {
            string jsonData;
            using var reader = new StreamReader(filePath);
            jsonData = reader.ReadToEnd();

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