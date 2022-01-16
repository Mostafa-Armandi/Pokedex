using System;
using Pokedex.Enums;

namespace Pokedex.ViewModels
{
    public class PokemonModel
    {
        private const string CaveHabitat = "cave";

        public string Name { get; set; }

        public string Description { get; set; }

        public string Habitat { get; set; }

        public bool IsLegendary { get; set; }

        public TranslatorSource GetTranslator() =>
            NeedsYodaTranslator() ? TranslatorSource.Yoda : TranslatorSource.Shakespeare;
        
        private bool NeedsYodaTranslator()
        {
            var caveInhabitant = Habitat.Equals(CaveHabitat, StringComparison.OrdinalIgnoreCase);
            var isLegendary = IsLegendary;

            return caveInhabitant || isLegendary;
        }
    }
}