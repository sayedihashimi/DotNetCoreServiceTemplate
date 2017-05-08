using Lykke.Assets;
using Lykke.Assets.Client;
using Lykke.Assets.Client.Helpers;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddLykkeAssetsClient(this IServiceCollection services,
            string apiUrl, string apiKey)
        {
            services.AddSingleton(new AssetsRestClient(apiUrl, apiKey));

            services.AddSingleton<IAssetsRepository, AssetsRepository>();
        }
    }
}