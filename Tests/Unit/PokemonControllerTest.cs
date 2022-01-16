using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Pokedex.Clients.Pokemon;
using Pokedex.Clients.Translator;
using Pokedex.Controllers;
using Pokedex.Enums;
using Pokedex.ViewModels;
using Xunit;

namespace Tests.Unit
{
    public class PokemonControllerTest
    {
        [Fact]
        private async Task GetByNameAsync_GivenEmptyPokemonName_ShouldReturnBadRequest()
        {
            var sut = new PokemonController(Mock.Of<IPokemonClient>(), Mock.Of<ITranslatorClient>(),
                Mock.Of<ILogger<PokemonController>>());

            var result = await sut.GetByNameAsync(string.Empty);

            result.Result.Should().BeOfType<BadRequestResult>();
            result.Value.Should().BeNull();
        }


        [Fact]
        private async Task GetByNameAsync_GivenValidPokemonName_ShouldReturnThePokemonResource()
        {
            var sampleModel = GetSamplePokemonModel();
            var sut = new PokemonController(GetPokemonClientMocked(sampleModel).Object,
                Mock.Of<ITranslatorClient>(),
                Mock.Of<ILogger<PokemonController>>());

            var result = await sut.GetByNameAsync(sampleModel.Name);

            result.Value.Should().BeEquivalentTo(sampleModel);
        }

        [Fact]
        private async Task GetTranslatedByNameAsync_GivenEmptyPokemonName_ShouldReturnBadRequest()
        {
            var invalidPokemonName = string.Empty;
            var sut = new PokemonController(Mock.Of<IPokemonClient>(), Mock.Of<ITranslatorClient>(),
                Mock.Of<ILogger<PokemonController>>());

            var result = await sut.GetTranslatedByNameAsync(invalidPokemonName);

            result.Result.Should().BeOfType<BadRequestResult>();
            result.Value.Should().BeNull();
            
        }
        
        [Fact]
        private async Task GetTranslatedByNameAsync_GivenPokemonName_ShouldTranslateDescription()
        {
            var sampleModel = GetSamplePokemonModel();
            var originalDescription = sampleModel.Description;
            var translatedDescription = "Translated Description";
            var sut = new PokemonController(
                GetPokemonClientMocked(sampleModel).Object,
                GetTranslatorClientMocked(originalDescription, translatedDescription).Object,
                Mock.Of<ILogger<PokemonController>>());

            var result = await sut.GetTranslatedByNameAsync(sampleModel.Name);

            result.Value.Should().BeEquivalentTo(sampleModel);
            result.Value.Description.Should().BeEquivalentTo(translatedDescription);
        }

        [Fact]
        private async Task GetTranslatedByNameAsync_FailingTranslation_ShouldRetainOriginalDescription()
        {
            var sampleModel = GetSamplePokemonModel();
            var originalDescription = sampleModel.Description;
            var translatedDescription = "Translated Description";
            var translatorClientMocked = GetTranslatorClientMocked(originalDescription, translatedDescription);
            translatorClientMocked.Setup(_ => _.TranslateAsync(It.IsAny<TranslatorSource>(), It.IsAny<string>()))
                .ThrowsAsync(It.IsAny<Exception>());

            var sut = new PokemonController(
                GetPokemonClientMocked(sampleModel).Object,
                translatorClientMocked.Object,
                Mock.Of<ILogger<PokemonController>>());
            
            var result = await sut.GetTranslatedByNameAsync(sampleModel.Name);

            result.Value.Description.Should().Be(originalDescription);
        }

        private PokemonModel GetSamplePokemonModel() =>
            new PokemonModel
            {
                Description = "sample description",
                Habitat = "cave",
                Name = "ditto",
                IsLegendary = true
            };

        private Mock<IPokemonClient> GetPokemonClientMocked(PokemonModel returnedResponse)
        {
            var client = new Mock<IPokemonClient>();
            client
                .Setup(_ => _.GetByNameAsync(returnedResponse.Name))
                .Returns(Task.FromResult(returnedResponse));

            return client;
        }

        private Mock<ITranslatorClient> GetTranslatorClientMocked(string original, string translated)
        {
            var client = new Mock<ITranslatorClient>();
            client
                .Setup(_ => _.TranslateAsync(It.IsAny<TranslatorSource>(), original))
                .Returns(Task.FromResult(translated));

            return client;
        }
    }
}