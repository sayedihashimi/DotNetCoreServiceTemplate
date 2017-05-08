using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.ApiAuth
{
    public interface ILykkeApp
    {
        string Id { get; }
        string Name { get; }
        string Description { get; }
        bool IsDisabled { get; }
    }

    public class LykkeApp : ILykkeApp
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public bool IsDisabled { get; set; }

        public static LykkeApp Create(ILykkeApp src)
        {
            return new LykkeApp
            {
                Id = src.Id,
                Name = src.Name,
                Description = src.Description,

                IsDisabled = src.IsDisabled
            };
        }
    }

    public interface ILykkeAppsRepository
    {
        Task CreateAsync(ILykkeApp model);
        Task EditAsync(ILykkeApp model);
        Task<ILykkeApp> GetAsync(string id);
        Task<IEnumerable<ILykkeApp>> GetAsync();
    }
}