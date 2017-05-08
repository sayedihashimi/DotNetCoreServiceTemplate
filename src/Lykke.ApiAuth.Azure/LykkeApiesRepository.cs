using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureStorage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.ApiAuth.Azure
{
    public class LykkeApiEntity : TableEntity, ILykkeApi
    {
        public const string Partition = "LykkeApi";

        public LykkeApiEntity()
        {
            PartitionKey = Partition;
        }

        public LykkeApiEntity(string id)
            : this()
        {
            RowKey = id;
        }

        public string Id => RowKey;
        public string Name { get; set; }

        public static LykkeApiEntity Create(ILykkeApi src)
        {
            return Map(src, new LykkeApiEntity(src.Id));
        }

        public static LykkeApiEntity Map(ILykkeApi src, LykkeApiEntity dest)
        {
            dest.Name = src.Name;

            return dest;
        }
    }

    public class LykkeApiesRepository : ILykkeApiesRepository
    {
        private readonly INoSQLTableStorage<LykkeApiEntity> _tableStorage;

        public LykkeApiesRepository(INoSQLTableStorage<LykkeApiEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public Task CreateAsync(ILykkeApi model)
        {
            var entity = LykkeApiEntity.Create(model);
            return _tableStorage.InsertAsync(entity);
        }

        public Task EditAsync(ILykkeApi model)
        {
            return _tableStorage.MergeAsync(LykkeApiEntity.Partition, model.Id,
                entity => LykkeApiEntity.Map(model, entity));
        }

        public async Task<ILykkeApi> GetAsync(string id)
        {
            var entity = await _tableStorage
                .GetDataAsync(LykkeApiEntity.Partition, id);

            return entity == null ? null
                : LykkeApi.Create(entity);
        }

        public async Task<IEnumerable<ILykkeApi>> GetAsync()
        {
            return (await _tableStorage
                    .GetDataAsync(LykkeApiEntity.Partition))
                .Select(LykkeApi.Create);
        }
    }
}