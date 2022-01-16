using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pokedex.Clients;
using Pokedex.Clients.Pokemon;
using Pokedex.Clients.Translator;

namespace Tests.Integration.Fixtures
{
    public class ApiWebApplicationFactory : WebApplicationFactory<Pokedex.Startup>
    {
        public IConfiguration Configuration { get; private set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(config =>
            {
                Configuration = new ConfigurationBuilder()
                    .Build();

                config.AddConfiguration(Configuration);
            });
            
            builder.ConfigureServices(services =>
            {
                services.AddTransient(typeof(IGenericClient<>), typeof(GenericClient<>));
                services.AddTransient<IPokemonClient, PokemonClientDefault>();
                services.AddTransient<ITranslatorClient, TranslatorClientDefault>();
            });
        }
    }
}