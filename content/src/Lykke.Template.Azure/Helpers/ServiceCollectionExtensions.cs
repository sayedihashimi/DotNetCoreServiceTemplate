using System;
using AzureStorage.Tables;
using Common.Log;
using Lykke.Template.Abstractions;
using Lykke.Template.Azure;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLykkeTemplateAzureRepositories(this IServiceCollection services,
            Action<LykkeTemplateAzureConfig> configurator)
        {
            var config = new LykkeTemplateAzureConfig();
            configurator.Invoke(config);

            return AddLykkeTemplateAzureRepositories(services, config);
        }

        public static IServiceCollection AddLykkeTemplateAzureRepositories(this IServiceCollection services,
            LykkeTemplateAzureConfig config)
        {
            services.AddSingleton<ISamplesRepository>(
                new SamplesRepository(new AzureTableStorage<SampleEntity>(
                    config.ConnectionString, "Samples", config.Logger)));

            return services;
        }
    }

    public class LykkeTemplateAzureConfig
    {
        public string ConnectionString { get; set; }
        public ILog Logger { get; set; }
    }
}