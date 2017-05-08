using System;
using System.Net.Http.Headers;
using Lykke.Http;

namespace Lykke.Assets.Client.Helpers
{
    public class AssetsRestClient : RestClient
    {
        public AssetsRestClient(string baseUrl, string apiKey)
        {
            HttpClient.BaseAddress = new Uri(baseUrl);

            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("ApiKey", apiKey);
        }
    }
}