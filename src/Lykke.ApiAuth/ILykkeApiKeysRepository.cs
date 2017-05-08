using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.ApiAuth
{
    public interface ILykkeApiKey
    {
        string Id { get; }
        string LykkeApiId { get; }
        string LykkeAppId { get; }
    }

    public class LykkeApiKey : ILykkeApiKey
    {
        public string Id { get; set; }
        public string LykkeApiId { get; set; }
        public string LykkeAppId { get; set; }

        public static LykkeApiKey Create(ILykkeApiKey src)
        {
            return new LykkeApiKey
            {
                Id = src.Id,
                LykkeApiId = src.LykkeApiId,
                LykkeAppId = src.LykkeAppId
            };
        }
    }

    public interface ILykkeApiKeysRepository
    {
        Task CreateAsync(ILykkeApiKey model);
        Task EditAsync(ILykkeApiKey model);
        Task<ILykkeApiKey> GetAsync(string id);
        Task<IEnumerable<ILykkeApiKey>> GetAsync();
    }
}