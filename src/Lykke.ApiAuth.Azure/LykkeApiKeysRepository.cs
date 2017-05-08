using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureStorage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.ApiAuth.Azure
{
    public class LykkeApiKeyEntity : TableEntity, ILykkeApiKey
    {
        public const string Partition = "LykkeApiKey";

        public LykkeApiKeyEntity()
        {
            PartitionKey = Partition;
        }

        public LykkeApiKeyEntity(string id)
            : this()
        {
            RowKey = id;
        }

        public string Id => RowKey;
        public string LykkeApiId { get; set; }
        public string LykkeAppId { get; set; }

        public static LykkeApiKeyEntity Create(ILykkeApiKey src)
        {
            return Map(src, new LykkeApiKeyEntity(src.Id));
        }

        public static LykkeApiKeyEntity Map(ILykkeApiKey src, LykkeApiKeyEntity dest)
        {
            dest.LykkeApiId = src.LykkeApiId;
            dest.LykkeAppId = src.LykkeAppId;

            return dest;
        }
    }

    public class LykkeApiKeysRepository : ILykkeApiKeysRepository
    {
        private readonly INoSQLTableStorage<LykkeApiKeyEntity> _tableStorage;

        public LykkeApiKeysRepository(INoSQLTableStorage<LykkeApiKeyEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public Task CreateAsync(ILykkeApiKey model)
        {
            var entity = LykkeApiKeyEntity.Create(model);
            return _tableStorage.InsertAsync(entity);
        }

        public Task EditAsync(ILykkeApiKey model)
        {
            return _tableStorage.MergeAsync(LykkeApiKeyEntity.Partition, model.Id,
                entity => LykkeApiKeyEntity.Map(model, entity));
        }

        public async Task<ILykkeApiKey> GetAsync(string id)
        {
            var entity = await _tableStorage
                .GetDataAsync(LykkeApiKeyEntity.Partition, id);

            return entity == null ? null
                : LykkeApiKey.Create(entity);
        }

        public async Task<IEnumerable<ILykkeApiKey>> GetAsync()
        {
            return (await _tableStorage
                    .GetDataAsync(LykkeApiKeyEntity.Partition))
                .Select(LykkeApiKey.Create);
        }
    }
}