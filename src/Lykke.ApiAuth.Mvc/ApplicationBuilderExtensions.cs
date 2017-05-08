using System;
using System.Threading.Tasks;
using Lykke.ApiAuth;
using Lykke.ApiAuth.Mvc;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseLykkeApiAuth(this IApplicationBuilder app, Action<ApiAuthConfig> configurator)
        {
            var config = new ApiAuthConfig();
            configurator.Invoke(config);
            UseLykkeApiAuth(app, config);
        }

        public static void UseLykkeApiAuth(this IApplicationBuilder app, ApiAuthConfig config)
        {
            app.UseMiddleware<ApiAuthMiddleware>(config);
        }

        public static async Task ConfigureTemplateAuth(this IApplicationBuilder app)
        {
            var apiesRepository = app.ApplicationServices.GetService<ILykkeApiesRepository>();
            var lykkeApi = await apiesRepository.GetAsync("02b56b9a-d639-45aa-83a2-efe481d0a51f");
            if (lykkeApi == null)
            {
                lykkeApi = new LykkeApi
                {
                    Id = "02b56b9a-d639-45aa-83a2-efe481d0a51f",
                    Name = "Template API"
                };
                await apiesRepository.CreateAsync(lykkeApi);
            }

            var appsRepository = app.ApplicationServices.GetService<ILykkeAppsRepository>();
            var lykkeApp = await appsRepository.GetAsync("e8c85e63-90eb-4a23-8282-a7945eb08655");
            if (lykkeApp == null)
            {
                lykkeApp = new LykkeApp
                {
                    Id = "e8c85e63-90eb-4a23-8282-a7945eb08655",
                    Name = "Example Client",
                    Description = "Example Client",
                };
                await appsRepository.CreateAsync(lykkeApp);
            }

            var apiKeysRepository = app.ApplicationServices.GetService<ILykkeApiKeysRepository>();
            var lykkeApiKey = await apiKeysRepository.GetAsync("f13db92c-d76f-41ac-a10a-f4fe9c120cb6");
            if (lykkeApiKey == null)
            {
                lykkeApiKey = new LykkeApiKey
                {
                    Id = "f13db92c-d76f-41ac-a10a-f4fe9c120cb6",
                    LykkeApiId = lykkeApi.Id,
                    LykkeAppId = lykkeApp.Id
                };
                await apiKeysRepository.CreateAsync(lykkeApiKey);
            }
        }
    }
}