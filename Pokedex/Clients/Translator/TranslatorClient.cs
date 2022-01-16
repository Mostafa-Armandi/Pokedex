using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Pokedex.Enums;

namespace Pokedex.Clients.Translator
{
    public class TranslatorClient : ITranslatorClient
    {
        private readonly IGenericClient<TranslatorResponse> _genericClient;
        private readonly RemoteApiUrls _urls;

        public TranslatorClient(IGenericClient<TranslatorResponse> genericClient, IOptions<RemoteApiUrls> urlOptions)
        {
            _genericClient = genericClient;
            _urls = urlOptions.Value;
        }

        public async Task<string> TranslateAsync(TranslatorSource translatorSource, string input)
        {
            var apiUrl = GetApiUrl(translatorSource) + input;

            var response = await _genericClient.GetAsync(apiUrl);

            return response.Contents.Translated;
        }

        private string GetApiUrl(TranslatorSource translatorSource)
        {
            return translatorSource switch
            {
                TranslatorSource.Shakespeare => _urls.ShakespeareApiUrl,
                TranslatorSource.Yoda => _urls.YodaApiUrl,
                _ => throw new ArgumentOutOfRangeException(nameof(translatorSource))
            };
        }
    }
}