using Newtonsoft.Json;

namespace Quizzify.Client.Settings.Language;

public class MainSettings
{
    [JsonProperty("Language")]
    public LanguageSettings LanguageSettings;

    public static bool IsNullOrEmpty(MainSettings settings)
    {
        return settings?.LanguageSettings == null;
    }
}
