using System.Text.Json.Serialization;

namespace Pokedex.Clients.Translator
{
    public class Contents
    {
        [JsonPropertyName("translated")]
        public string Translated { get; set; }
    }
    
    public class TranslatorResponse
    {
        [JsonPropertyName("contents")]
        public Contents Contents { get; set; }
    }
}