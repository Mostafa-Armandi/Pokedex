using System.Threading.Tasks;
using Pokedex.ViewModels;

namespace Pokedex.Clients.Pokemon
{
    public interface IPokemonClient
    {
        Task<PokemonModel> GetByNameAsync(string pokemonName);
    }
}