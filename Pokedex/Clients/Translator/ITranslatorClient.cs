using System.Threading.Tasks;
using Pokedex.Enums;

namespace Pokedex.Clients.Translator
{
    public interface ITranslatorClient
    {
        Task<string> TranslateAsync(TranslatorSource translatorSource, string input);
    }
}