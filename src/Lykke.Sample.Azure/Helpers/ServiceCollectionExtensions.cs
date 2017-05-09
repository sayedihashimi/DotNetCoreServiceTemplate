using System;
using AzureStorage.Tables;
using Lykke.Sample;
using Lykke.Sample.Azure;
using Lykke.Sample.Azure.Helpers;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLykkeSampleAzure(this IServiceCollection services,
            Action<LykkeSampleAzureConfig> configurator)
        {
            var config = new LykkeSampleAzureConfig();
            configurator.Invoke(config);

            return AddLykkeSampleAzure(services, config);
        }

        public static IServiceCollection AddLykkeSampleAzure(this IServiceCollection services,
            LykkeSampleAzureConfig config)
        {
            services.AddSingleton<ISamplesRepository>(
                new SamplesRepository(new AzureTableStorage<SampleEntity>(
                    config.ConnectionString, "Samples", config.Logger)));

            return services;
        }
    }
}