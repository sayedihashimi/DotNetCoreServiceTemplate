using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Lykke.Http
{
    public class RestClient
    {
        protected readonly HttpClient HttpClient;

        public RestClient()
        {
            HttpClient = new HttpClient();
        }

        public async Task<Result<T>> GetResultAsync<T>(string address)
        {
            var response = await HttpClient.GetAsync(address);

            var content = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(content))
                return new Result<T>
                {
                    Status = GetResultStatus(response)
                };

            return JsonConvert.DeserializeObject<Result<T>>(content);
        }

        public async Task<Result> PostAsync(string address, object model)
        {
            var httpContent = PrepareContent(model);
            var response = await HttpClient.PostAsync(address, httpContent);

            var content = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(content))
                return new Result
                {
                    Status = GetResultStatus(response)
                };

            return JsonConvert.DeserializeObject<Result>(content);
        }

        public async Task<Result> PutAsync(string address, object model)
        {
            var httpContent = PrepareContent(model);
            var response = await HttpClient.PutAsync(address, httpContent);

            var content = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(content))
                return new Result
                {
                    Status = GetResultStatus(response)
                };

            return JsonConvert.DeserializeObject<Result>(content);
        }

        private static StringContent PrepareContent(object model)
        {
            var body = JsonConvert.SerializeObject(model);
            var httpContent = new StringContent(body);
            httpContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            return httpContent;
        }

        private static ResultStatus GetResultStatus(HttpResponseMessage response)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                case HttpStatusCode.Created:
                case HttpStatusCode.NoContent:
                    return ResultStatus.Success;

                case HttpStatusCode.NotFound:
                    return ResultStatus.NotFound;

                case HttpStatusCode.BadRequest:
                    return ResultStatus.BadRequest;

                case HttpStatusCode.Unauthorized:
                    return ResultStatus.UnAuthenticated;
                case HttpStatusCode.Forbidden:
                    return ResultStatus.UnAuthorized;

                default:
                    return response.IsSuccessStatusCode
                        ? ResultStatus.Success
                        : ResultStatus.Error;
            }
        }
    }
}