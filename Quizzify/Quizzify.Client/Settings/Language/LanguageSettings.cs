using Newtonsoft.Json;

namespace Quizzify.Client.Settings.Language;
public class LanguageSettings
{
    [JsonProperty("Code")]
    public string LanguageCode { get; set; }
}
