using System;
using Lykke.Sample;
using Lykke.Sample.WebClient;
using Lykke.Sample.WebClient.Helpers;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLykkeSampleClient(this IServiceCollection services,
            string apiUrl, string apiKey)
        {
            return AddLykkeSampleClient(services, config =>
            {
                config.ApiUrl = apiUrl;
                config.ApiKey = apiKey;
            });
        }

        public static IServiceCollection AddLykkeSampleClient(this IServiceCollection services,
            Action<SampleRestClientConfig> configurator)
        {
            var config = new SampleRestClientConfig();
            configurator.Invoke(config);

            return AddLykkeSampleClient(services, config);
        }

        public static IServiceCollection AddLykkeSampleClient(this IServiceCollection services,
            SampleRestClientConfig config)
        {
            services.AddSingleton(config);
            services.AddSingleton<ISamplesRepository, SamplesRepositoryClient>();

            return services;
        }
    }
}