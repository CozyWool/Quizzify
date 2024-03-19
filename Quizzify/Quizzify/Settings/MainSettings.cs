using Newtonsoft.Json;

namespace Quizzify.Settings;

public class MainSettings
{
    [JsonProperty("Language")]
    public LanguageSettings languageSettings;
}
