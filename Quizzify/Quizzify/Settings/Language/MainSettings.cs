using Newtonsoft.Json;

namespace Quizzify.Settings.Language;

public class MainSettings
{
    [JsonProperty("Language")]
    public LanguageSettings languageSettings;
}
