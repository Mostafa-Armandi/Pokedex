using System.Threading.Tasks;
using Pokedex.Clients;

namespace Tests.Integration.Fixtures
{
    public class DefaultGenericClientStub<TResult> : IGenericClient<TResult>
    {
        public Task<TResult> GetAsync(string url)
        {
            return Task.FromResult<TResult>(default);
        }
    }
}