using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.ApiAuth
{
    public interface ILykkeApi
    {
        string Id { get; }
        string Name { get; }
    }

    public class LykkeApi : ILykkeApi
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public static LykkeApi Create(ILykkeApi src)
        {
            return new LykkeApi
            {
                Id = src.Id,
                Name = src.Name
            };
        }
    }

    public interface ILykkeApiesRepository
    {
        Task CreateAsync(ILykkeApi model);
        Task EditAsync(ILykkeApi model);
        Task<ILykkeApi> GetAsync(string id);
        Task<IEnumerable<ILykkeApi>> GetAsync();
    }
}