using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Lykke.Http
{
    public class RestClient
    {
        protected readonly HttpClient HttpClient;

        public RestClient(RestClientConfig config)
            : this (config.ApiUrl, config.ApiKey) { }

        public RestClient(string apiUrl, string apiKey)
        {
            HttpClient = new HttpClient
            {
                BaseAddress = new Uri(apiUrl)
            };

            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("ApiKey", apiKey);
        }

        public async Task<T> GetResultAsync<T>(string address)
        {
            var response = await HttpClient.GetAsync(address);
            return await ExportResult<T>(response);
        }

        public async Task<T> PostAsync<T>(string address, object model)
        {
            var httpContent = PrepareContent(model);
            var response = await HttpClient.PostAsync(address, httpContent);
            return await ExportResult<T>(response);
        }

        public async Task PostAsync(string address, object model)
        {
            var httpContent = PrepareContent(model);
            var response = await HttpClient.PostAsync(address, httpContent);
            await ExportResult(response);
        }

        public async Task<T> PutAsync<T>(string address, object model)
        {
            var httpContent = PrepareContent(model);
            var response = await HttpClient.PutAsync(address, httpContent);
            return await ExportResult<T>(response);
        }
        
        public async Task PutAsync(string address, object model)
        {
            var httpContent = PrepareContent(model);
            var response = await HttpClient.PutAsync(address, httpContent);
            await ExportResult(response);
        }

        private static async Task<T> ExportResult<T>(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
                throw await ExportException(response);

            var content = await response.Content.ReadAsStringAsync();

            return !string.IsNullOrEmpty(content)
                ? JsonConvert.DeserializeObject<T>(content)
                : default(T);
        }

        private static async Task ExportResult(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
                throw await ExportException(response);
        }

        private static async Task<Exception> ExportException(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(content))
                return new Exception();

            return JsonConvert.DeserializeObject<RestException>(content)?.ToExecption()
                ?? new Exception();
        }

        private static StringContent PrepareContent(object model)
        {
            var body = JsonConvert.SerializeObject(model);
            var httpContent = new StringContent(body);
            httpContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            return httpContent;
        }
    }
}