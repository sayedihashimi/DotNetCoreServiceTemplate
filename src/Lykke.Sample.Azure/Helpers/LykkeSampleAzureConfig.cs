using Common.Log;

namespace Lykke.Sample.Azure.Helpers
{
    public class LykkeSampleAzureConfig
    {
        public string ConnectionString { get; set; }
        public ILog Logger { get; set; }
    }
}