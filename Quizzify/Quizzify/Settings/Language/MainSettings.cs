using Newtonsoft.Json;

namespace Quizzify.Settings.Language;

public class MainSettings
{
    [JsonProperty("Language")]
    public LanguageSettings languageSettings;

    public static bool IsNullOrEmpty(MainSettings settings)
    {
        return settings == null || settings.languageSettings == null;
    }
}
