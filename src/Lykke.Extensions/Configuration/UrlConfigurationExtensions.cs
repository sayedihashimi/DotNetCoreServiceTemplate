using System;
using Microsoft.Extensions.Configuration;

namespace Lykke.Extensions.Configuration
{
    public static class UrlConfigurationExtensions
    {
        public static IConfigurationBuilder AddFromConfiguredUrl(this IConfigurationBuilder builder,
            string environmentVariableName)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            if (string.IsNullOrEmpty(environmentVariableName))
            {
                throw new ArgumentNullException(nameof(environmentVariableName));
            }

            var settingsUrl = Environment.GetEnvironmentVariable(environmentVariableName);
            if (string.IsNullOrEmpty(settingsUrl))
            {
                throw new Exception($"No Environment variable set for: {environmentVariableName}");
            }

            return AddFromUrl(builder, settingsUrl);
        }

        public static IConfigurationBuilder AddFromUrl(this IConfigurationBuilder builder, string settingsUrl)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            if (string.IsNullOrEmpty(settingsUrl))
            {
                throw new ArgumentNullException(nameof(settingsUrl));
            }
            
            var source = new UrlConfigurationSource
            {
                SettingsUrl = settingsUrl
            };
            builder.Add(source);
            return builder;
        }
    }
}