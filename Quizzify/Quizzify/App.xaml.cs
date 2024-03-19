using Newtonsoft.Json;
using Quizzify.Settings;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows;

namespace Quizzify;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        try
        {
            var userProfileDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var projectName = Assembly.GetExecutingAssembly().GetName().Name;

            var userDirectory = Path.Combine(userProfileDirectory, "AppData", "Local", projectName, "Locales");
            var filePath = Path.Combine(userDirectory, "appsettings.json");


            /*
             * Код с директорией временный. В будущем это переедет во viewmodel по смене языка. Но если самостоятельно создать appsettings.json с таким содержими:
             * {
             *  "Language": {
             *      "Code": "en-US"
             *   }
             * }
             * меняя значение на ru-RU - локаль приложения будет меняться на русский. Меняя обратно на en-US - локаль будет возвращена на английский.
            */

            if (!Directory.Exists(userDirectory))
            {
                Directory.CreateDirectory(userDirectory);
            }

            else if (File.Exists(filePath))
            {
                var jsonData = File.ReadAllText(filePath);
                var mainSettings = JsonConvert.DeserializeObject<MainSettings>(jsonData);

                string languageCode = mainSettings?.languageSettings.LangCode;

                if (!string.IsNullOrEmpty(languageCode))
                {
                    SetApplicationLanguage(languageCode);
                }
            }
            else
            {
                ShowMessage("Файл локализации не найден.");
            }

        }
        catch (Exception ex)
        {
            ShowMessage($"Произошла ошибка при загрузке настроек: {ex.Message}");
        }
        base.OnStartup(e);
    }

    private void SetApplicationLanguage(string languageCode)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo(languageCode);
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(languageCode);
    }

    private void ShowMessage(string message)
    {
        MessageBox.Show(message);
    }
}