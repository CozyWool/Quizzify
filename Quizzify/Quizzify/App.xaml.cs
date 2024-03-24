using Newtonsoft.Json;
using Quizzify.Settings.Language;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows;

namespace Quizzify;

public partial class App : Application
{
    private const string appDataFolder = "AppData";
    private const string localFolder = "Local";
    private const string settingsFile = "appsettings.json";
    private const string localesFolder = "Locales";

    protected override void OnStartup(StartupEventArgs e)
    {
        ApplyCultureFromSettingsFile();
        base.OnStartup(e);
    }

    private void ApplyCultureFromSettingsFile()
    {
        var filePath = GetLocalizationFilePath();
        if (!string.IsNullOrEmpty(filePath))
        {
            var mainSettings = LoadMainSettings(filePath);
            if (!MainSettings.IsNullOrEmpty(mainSettings))
            {
                var languageCode = mainSettings?.languageSettings.LangCode;
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
        var localizationFolderPath = Path.Combine(userProfileDirectory, appDataFolder, localFolder, projectName, localesFolder);
        var filePath = Path.Combine(localizationFolderPath, settingsFile);

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