using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pokedex.Clients.Pokemon;
using Pokedex.Clients.Translator;
using Pokedex.Filters;
using Pokedex.ViewModels;

namespace Pokedex.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [TypeFilter(typeof(HttpResponseExceptionFilter))]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonClient _pokemonClient;
        private readonly ITranslatorClient _translatorClient;
        private readonly ILogger<PokemonController> _logger;
        
        public PokemonController(IPokemonClient pokemonClient, ITranslatorClient translatorClient,
            ILogger<PokemonController> logger)
        {
            _pokemonClient = pokemonClient;
            _translatorClient = translatorClient;
            _logger = logger;
        }

        [HttpGet]
        [Route("{pokemonName}")]
        public async Task<ActionResult<PokemonModel>> GetByNameAsync(string pokemonName)
        {
            if (string.IsNullOrEmpty(pokemonName))
                return BadRequest();

            return await _pokemonClient.GetByNameAsync(pokemonName);
        }

        [HttpGet]
        [Route("translated/{pokemonName}")]
        public async Task<ActionResult<PokemonModel>> GetTranslatedByNameAsync(string pokemonName)
        {
            if (string.IsNullOrEmpty(pokemonName))
                return BadRequest();

            var pokemon = await _pokemonClient.GetByNameAsync(pokemonName);
            try
            {
                pokemon.Description =
                    await _translatorClient.TranslateAsync(pokemon.GetTranslator(), pokemon.Description);
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, $"Failed to translate text: {pokemon.Description}");
            }

            return pokemon;
        }
    }
}