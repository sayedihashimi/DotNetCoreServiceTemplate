using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Lykke.Http;

namespace Lykke.Assets.Host.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class AssetsController : Controller
    {
        private readonly IAssetsRepository _assetsRepository;

        public AssetsController(IAssetsRepository assetsRepository)
        {
            _assetsRepository = assetsRepository;
        }

        [HttpPost]
        public async Task<Result> RegisterAssetAsync([FromBody] Asset model)
        {
            await _assetsRepository.RegisterAssetAsync(model);
            return new Result {Status = ResultStatus.Success};
        }

        [HttpPut("{id}")]
        public async Task<Result> EditAssetAsync(string id, [FromBody] Asset asset)
        {
            await _assetsRepository.EditAssetAsync(id, asset);
            return new Result {Status = ResultStatus.Success};
        }

        [HttpPut("{id}/disabled/{value}")]
        public async Task<Result> SetDisabled(string id, bool value)
        {
            await _assetsRepository.SetDisabled(id, value);
            return new Result {Status = ResultStatus.Success};
        }

        [HttpGet]
        public async Task<Result<IEnumerable<IAsset>>> GetAssetsAsync([FromQuery] string category = null)
        {
            var list = string.IsNullOrEmpty(category)
                ? await _assetsRepository.GetAssetsAsync()
                : await _assetsRepository.GetAssetsForCategoryAsync(category);

            return new Result<IEnumerable<IAsset>>
            {
                Value = list,
                Status = ResultStatus.Success
            };
        }

        [HttpGet("{id}")]
        public async Task<Result<IAsset>> GetAssetAsync(string id)
        {
            var item = await _assetsRepository.GetAssetAsync(id);
            return new Result<IAsset>
            {
                Value = item,
                Status = item != null
                    ? ResultStatus.Success
                    : ResultStatus.NotFound
            };
        }
    }
}