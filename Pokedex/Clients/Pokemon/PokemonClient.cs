using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Pokedex.ViewModels;

namespace Pokedex.Clients.Pokemon
{
    public class PokemonClient : IPokemonClient
    {
        private readonly IGenericClient<PokemonResponse> _genericClient;
        private readonly RemoteApiUrls _urls;

        public PokemonClient(IGenericClient<PokemonResponse> genericClient, IOptions<RemoteApiUrls> urlOptions)
        {
            _genericClient = genericClient;
            _urls = urlOptions.Value;
        }

        public async Task<PokemonModel> GetByNameAsync(string pokemonName)
        {
            var apiUrl = _urls.PokemonApiUrl + pokemonName;

            var response = await _genericClient.GetAsync(apiUrl);

            return new PokemonModel
            {
                Description = response.Description,
                Habitat = response.HabitatName,
                Name = response.Name,
                IsLegendary = response.IsLegendary
            };
        }
    }
}