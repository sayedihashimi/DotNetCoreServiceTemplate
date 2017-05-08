using System.Net.Http;
using Microsoft.Extensions.Configuration;

namespace Lykke.Extensions.Configuration
{
    public class UrlConfigurationProvider : ConfigurationProvider
    {
        private readonly UrlConfigurationSource _source;

        public UrlConfigurationProvider(UrlConfigurationSource source)
        {
            _source = source;
        }

        public override void Load()
        {
            var client = new HttpClient();
            using (var stream = client.GetStreamAsync(_source.SettingsUrl).GetAwaiter().GetResult())
            {
                var parser = new JsonConfigurationParser();
                Data = parser.Parse(stream);
            }
        }
    }
}