using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Pokedex.Clients.Pokemon
{
    public class PokemonResponse
    {
        public string Name { get; set; }

        [JsonPropertyName("is_legendary")] 
        public bool IsLegendary { get; set; }

        public Habitat Habitat { get; set; }

        [JsonPropertyName("flavor_text_entries")]
        public List<FlavorTextEntry> FlavorTextEntries { get; set; }

        public string Description =>
            FlavorTextEntries
                .FirstOrDefault(_ => _.Language.Name.ToLower() == "en")?.FlavorText
                .Replace("\n", " ")
                .Replace("\f", " ")
                .Replace("\r", " ");

        public string HabitatName => Habitat.Name;
    }

    public class Habitat
    {
        public string Name { get; set; }
    }

    public class Language
    {
        public string Name { get; set; }
    }

    public class FlavorTextEntry
    {
        [JsonPropertyName("flavor_text")] 
        public string FlavorText { get; set; }

        public Language Language { get; set; }
    }
}