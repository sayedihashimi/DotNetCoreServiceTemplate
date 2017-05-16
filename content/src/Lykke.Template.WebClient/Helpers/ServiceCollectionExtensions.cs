using System;
using Lykke.Http;
using Lykke.Template.Abstractions;
using Lykke.Template.WebClient;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLykkeTemplateWebClient(this IServiceCollection services,
            string apiUrl, string apiKey)
        {
            return AddLykkeTemplateWebClient(services, config =>
            {
                config.ApiUrl = apiUrl;
                config.ApiKey = apiKey;
            });
        }

        public static IServiceCollection AddLykkeTemplateWebClient(this IServiceCollection services,
            Action<LykkeTemplateRestClientConfig> configurator)
        {
            var config = new LykkeTemplateRestClientConfig();
            configurator.Invoke(config);

            return AddLykkeTemplateWebClient(services, config);
        }

        public static IServiceCollection AddLykkeTemplateWebClient(this IServiceCollection services,
            LykkeTemplateRestClientConfig config)
        {
            services.AddSingleton(config);
            services.AddSingleton<ISamplesRepository, SamplesRepositoryClient>();

            return services;
        }
    }
}

namespace Lykke.Template.WebClient
{
    public class LykkeTemplateRestClientConfig : RestClientConfig
    {

    }
}