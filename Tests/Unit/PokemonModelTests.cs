using FluentAssertions;
using Pokedex.Enums;
using Pokedex.ViewModels;
using Xunit;

namespace Tests.Unit
{
    public class PokemonModelTest
    {
        [Theory]
        [InlineData("cave", true, TranslatorSource.Yoda)]
        [InlineData("cave", false, TranslatorSource.Yoda)]
        [InlineData("tree", true, TranslatorSource.Yoda)]
        [InlineData("tree", false, TranslatorSource.Shakespeare)]
        public void GetTranslator_ReturnsTheCorrectTranslatorSource(string habitat, bool isLegendary,
            TranslatorSource expected)
        {
            var sut = new PokemonModel
            {
                Habitat = habitat,
                IsLegendary = isLegendary
            };

            sut.GetTranslator().Should().Be(expected);
        }
    }
}