using System.Threading.Tasks;
using Pokedex.Clients.Translator;
using Pokedex.Enums;

namespace Tests.Integration.Fixtures
{
    public class TranslatorClientDefault : ITranslatorClient
    {
        public Task<string> TranslateAsync(TranslatorSource translatorSource, string input)
        {
            return Task.FromResult(input);
        }
    }
}