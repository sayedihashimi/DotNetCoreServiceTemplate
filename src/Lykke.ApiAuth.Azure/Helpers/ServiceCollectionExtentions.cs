using System;
using AzureStorage.Tables;
using Common.Log;
using Lykke.ApiAuth;
using Lykke.ApiAuth.Azure;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class LykkeAuthAzureServiceCollectionExtensions
    {
        //public static void AddLykkeApiAuthAzure(this IServiceCollection services,
        //    IConfigurationSection configurationSection, ILog log)
        //{
        //    var config = new ApiAuthAzureConfig
        //    {
        //        ConnectionString = configurationSection.get
        //    };
        //}

        public static void AddLykkeApiAuthAzure(this IServiceCollection services,
            Action<ApiAuthAzureConfig> configurator, ILog log)
        {
            var config = new ApiAuthAzureConfig();
            configurator.Invoke(config);

            AddLykkeApiAuthAzure(services, config, log);
        }

        public static void AddLykkeApiAuthAzure(this IServiceCollection services,
            ApiAuthAzureConfig config, ILog log)
        {
            services.AddSingleton<ILykkeApiesRepository>(
                new LykkeApiesRepository(new AzureTableStorage<LykkeApiEntity>(
                    config.ConnectionString, "ApiAuth", log)));

            services.AddSingleton<ILykkeAppsRepository>(
                new LykkeAppsRepository(new AzureTableStorage<LykkeAppEntity>(
                    config.ConnectionString, "ApiAuth", log)));

            services.AddSingleton<ILykkeApiKeysRepository>(
                new LykkeApiKeysRepository(new AzureTableStorage<LykkeApiKeyEntity>(
                    config.ConnectionString, "ApiAuth", log)));
        }
    }

    public class ApiAuthAzureConfig
    {
        public string ConnectionString { get; set; }
    }
}