using System.Threading.Tasks;

namespace Pokedex.Clients
{
    public interface IGenericClient<TResult>
    {
        Task<TResult> GetAsync(string url);
    }
}