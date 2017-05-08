using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Assets.Client.Helpers;

namespace Lykke.Assets.Client
{
    public class AssetsRepository : IAssetsRepository
    {
        private const string Endpoint = "api/assets";
        private readonly AssetsRestClient _assetsRestClient;

        public AssetsRepository(AssetsRestClient assetsRestClient)
        {
            _assetsRestClient = assetsRestClient;
        }

        public Task RegisterAssetAsync(IAsset asset)
        {
            return _assetsRestClient.PostAsync(Endpoint, asset);
        }

        public Task EditAssetAsync(string id, IAsset asset)
        {
            return _assetsRestClient.PutAsync($"{Endpoint}/{id}", asset);
        }

        public Task SetDisabled(string id, bool value)
        {
            return _assetsRestClient.PutAsync(
                $"{Endpoint}/{id}/disabled/{value}", new {});
        }

        public async Task<IAsset> GetAssetAsync(string id)
        {
            var result = await _assetsRestClient
                   .GetResultAsync<Asset>($"{Endpoint}/{id}");

            return result.Value;
        }

        public async Task<IEnumerable<IAsset>> GetAssetsAsync()
        {
            var result = await _assetsRestClient
                .GetResultAsync<IEnumerable<Asset>>(Endpoint);

            return result.Value;
        }

        public async Task<IEnumerable<IAsset>> GetAssetsForCategoryAsync(string category)
        {
            var result = await _assetsRestClient
                .GetResultAsync<IEnumerable<Asset>>($"{Endpoint}/?category={category}");

            return result.Value;
        }
    }
}