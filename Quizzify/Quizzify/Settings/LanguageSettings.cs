using Newtonsoft.Json;

namespace Quizzify.Settings;
public class LanguageSettings
{
    [JsonProperty("Code")]
    public string LangCode { get; set; }
}
