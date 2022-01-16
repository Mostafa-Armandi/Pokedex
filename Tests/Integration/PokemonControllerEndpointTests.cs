using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Pokedex.Clients.Pokemon;
using Pokedex.Exceptions;
using Pokedex.ViewModels;
using Tests.Integration.Fixtures;
using Xunit;

namespace Tests.Integration
{
    public class PokemonControllerEndpointTests : IntegrationTest
    {
        public static PokemonModel SampleModel => new PokemonModel()
        {
            Description = "sample description",
            Habitat = "cave",
            Name = "ditto",
            IsLegendary = true
        };

        public const HttpStatusCode NoFoundStatusCode = HttpStatusCode.NotFound;

        public PokemonControllerEndpointTests(ApiWebApplicationFactory fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task PokemonEndPoint_GivenAValidPokemonName_ShouldOutputTheExpectedResult()
        {
            var client = _factory.CreateClient();

            var model = await client.GetFromJsonAsync<PokemonModel>("/pokemon/ditto");

            model.Should().BeEquivalentTo(SampleModel);
        }

        [Fact]
        public async Task PokemonTranslatedEndPoint_GivenAValidPokemonName_ShouldOutputTheExpectedResult()
        {
            var client = _factory.CreateClient();

            var model = await client.GetFromJsonAsync<PokemonModel>("/pokemon/translated/ditto");

            model.Should().BeEquivalentTo(SampleModel);
        }

        [Fact]
        public async Task PokemonEndPoint_GetUnsuccessfulApiResponse_ShouldPassThroughTheStatusCode()
        {
            var client = _factory.WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.AddTransient<IPokemonClient, PokemonClientWithNotFoundException>();
                    });
                })
                .CreateClient();

            var response = await client.GetAsync("/pokemon/ditto");

            response.StatusCode.Should().Be(NoFoundStatusCode);
        }
    }

    public class PokemonClientWithNotFoundException : IPokemonClient
    {
        public Task<PokemonModel> GetByNameAsync(string pokemonName)
        {
            throw new RemoteApiException(PokemonControllerEndpointTests.NoFoundStatusCode, string.Empty);
        }
    }
}