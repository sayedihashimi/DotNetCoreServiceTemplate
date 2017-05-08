using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureStorage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.ApiAuth.Azure
{
    public class LykkeAppEntity : TableEntity, ILykkeApp
    {
        public const string Partition = "LykkeApp";

        public LykkeAppEntity()
        {
            PartitionKey = Partition;
        }

        public LykkeAppEntity(string id)
            : this()
        {
            RowKey = id;
        }

        public string Id => RowKey;
        public string Name { get; set; }
        public string Description { get; set; }

        public bool IsDisabled { get; set; }
        
        public static LykkeAppEntity Create(ILykkeApp src)
        {
            return Map(src, new LykkeAppEntity(src.Id));
        }

        public static LykkeAppEntity Map(ILykkeApp src, LykkeAppEntity dest)
        {
            dest.Name = src.Name;
            dest.Description = src.Description;
            dest.IsDisabled = src.IsDisabled;

            return dest;
        }
    }

    public class LykkeAppsRepository : ILykkeAppsRepository
    {
        private readonly INoSQLTableStorage<LykkeAppEntity> _tableStorage;

        public LykkeAppsRepository(INoSQLTableStorage<LykkeAppEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public Task CreateAsync(ILykkeApp model)
        {
            var entity = LykkeAppEntity.Create(model);
            return _tableStorage.InsertAsync(entity);
        }

        public Task EditAsync(ILykkeApp model)
        {
            return _tableStorage.MergeAsync(LykkeAppEntity.Partition, model.Id,
                entity => LykkeAppEntity.Map(model, entity));
        }

        public async Task<ILykkeApp> GetAsync(string id)
        {
            var entity = await _tableStorage
                .GetDataAsync(LykkeAppEntity.Partition, id);

            return entity == null ? null
                : LykkeApp.Create(entity);
        }

        public async Task<IEnumerable<ILykkeApp>> GetAsync()
        {
            return (await _tableStorage
                    .GetDataAsync(LykkeAppEntity.Partition))
                .Select(LykkeApp.Create);
        }
    }
}