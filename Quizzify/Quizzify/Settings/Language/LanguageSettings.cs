using Newtonsoft.Json;

namespace Quizzify.Settings.Language;
public class LanguageSettings
{
    [JsonProperty("Code")]
    public string LangCode { get; set; }
}
