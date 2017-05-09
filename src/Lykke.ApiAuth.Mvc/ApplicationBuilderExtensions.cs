using System;
using Lykke.ApiAuth.Mvc;

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
    }
}