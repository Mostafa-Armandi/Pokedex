using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Moq.Protected;
using Pokedex.Clients;
using Pokedex.Clients.Pokemon;
using Pokedex.Exceptions;
using Xunit;

namespace Tests.Unit
{
    public class GenericClientTests
    {
        private readonly string _sampleUrl = "https://localhost.com";

        [Fact]
        public async Task GivenSuccessfulResponse_WhenGetPokemon_ShouldReturnTheRequestedObject()
        {
            var responseModel = GetSamplePokemonResponse();
            var httpClientFactoryMock = MockHttpClientFactory(responseModel, HttpStatusCode.OK);

            var sut = new GenericClient<PokemonResponse>(httpClientFactoryMock.Object);
            var result = await sut.GetAsync(_sampleUrl);

            result.Should().BeEquivalentTo(responseModel);
        }

        [Fact]
        public void GivenFailedResponse_WhenGetPokemon_ShouldThrowException()
        {
            var responseModel = GetSamplePokemonResponse();
            var failingStatusCode = HttpStatusCode.NotFound;
            var httpClientFactoryMock = MockHttpClientFactory(responseModel, failingStatusCode);

            var sut = new GenericClient<PokemonResponse>(httpClientFactoryMock.Object);

            sut.Invoking(s => s.GetAsync(_sampleUrl))
                .Should()
                .ThrowAsync<RemoteApiException>()
                .Where(e => e.StatusCode == failingStatusCode);
        }

        [Fact]
        public async Task GivenSuccessfulResponse_WhenGetPokemon_ShouldDeserializeDisregardingTheCaseSensitivity()
        {
            var loweCasedResponseModel = GetSamplePokemonResponse();
            var responseModel = GetSamplePokemonResponse();
            var httpClientFactoryMock = MockHttpClientFactory(loweCasedResponseModel, HttpStatusCode.OK, true);

            var sut = new GenericClient<PokemonResponse>(httpClientFactoryMock.Object);
            var result = await sut.GetAsync(_sampleUrl);

            result.Should().BeEquivalentTo(responseModel);
        }

        private PokemonResponse GetSamplePokemonResponse() =>
            new PokemonResponse
            {
                Habitat = new Habitat { Name = "cave" },
                Name = "ditto",
                IsLegendary = true,
                FlavorTextEntries = new List<FlavorTextEntry>
                {
                    new FlavorTextEntry
                    {
                        Language = new Language { Name = "en" },
                        FlavorText = "sample description"
                    }
                }
            };

        private Mock<IHttpClientFactory> MockHttpClientFactory(object responseText, HttpStatusCode statusCode,
            bool useCamelCase = false)
        {
            var mockFactory = new Mock<IHttpClientFactory>();

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            var serializeOptions = new JsonSerializerOptions();
            if (useCamelCase)
                serializeOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = new StringContent(JsonSerializer.Serialize(responseText, serializeOptions)),
                });


            var client = new HttpClient(mockHttpMessageHandler.Object);
            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);
            return mockFactory;
        }
    }
}