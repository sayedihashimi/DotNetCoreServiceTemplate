using Microsoft.Extensions.Configuration;

namespace Lykke.Extensions.Configuration
{
    public class UrlConfigurationSource : IConfigurationSource
    {
        public string SettingsUrl { get; set; }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new UrlConfigurationProvider(this);
        }
    }
}
