using AzureStorage.Tables;
using Common.Log;
using Lykke.Assets;
using Lykke.Assets.Azure;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddLykkeAssetsAzure(this IServiceCollection services,
            string connectionString, ILog log)
        {
            services.AddSingleton<IAssetsRepository>(
                new AssetsRepository(new AzureTableStorage<AssetEntity>(
                    connectionString, "Dictionaries", log)));
        }
    }
}