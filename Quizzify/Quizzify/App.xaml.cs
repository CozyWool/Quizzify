using Newtonsoft.Json;
using Quizzify.Settings.Language;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows;

namespace Quizzify;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        ApplyCultureFromSettingsFile();
        base.OnStartup(e);
    }

    private void ApplyCultureFromSettingsFile()
    {
        try
        {
            var filePath = GetLocalizationFilePath();

            if (!string.IsNullOrEmpty(filePath))
            {
                var mainSettings = LoadMainSettings(filePath);
                if (mainSettings != null)
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
        catch (Exception ex)
        {
            ShowMessage($"Произошла ошибка при загрузке настроек: {ex.Message}");
        }
    }

    private string GetLocalizationFilePath()
    {
        var userProfileDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var projectName = Assembly.GetExecutingAssembly().GetName().Name;
        var userDirectory = Path.Combine(userProfileDirectory, "AppData", "Local", projectName, "Locales");
        var filePath = Path.Combine(userDirectory, "appsettings.json");

        return File.Exists(filePath) ? filePath : null;
    }

    private MainSettings LoadMainSettings(string filePath)
    {
        string jsonData;
        using var reader = new StreamReader(filePath);
        jsonData = reader.ReadToEnd();

        return JsonConvert.DeserializeObject<MainSettings>(jsonData);
    }

    private void SetApplicationLanguage(string languageCode)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo(languageCode);
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(languageCode);
    }

    private void ShowMessage(string message) => MessageBox.Show(message);
}