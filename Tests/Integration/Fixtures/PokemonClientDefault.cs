using System.Threading.Tasks;
using Pokedex.Clients.Pokemon;
using Pokedex.ViewModels;

namespace Tests.Integration.Fixtures
{
    public class PokemonClientDefault : IPokemonClient
    {
        public Task<PokemonModel> GetByNameAsync(string pokemonName)
        {
            return Task.FromResult(PokemonControllerEndpointTests.SampleModel);
        }
    }
}