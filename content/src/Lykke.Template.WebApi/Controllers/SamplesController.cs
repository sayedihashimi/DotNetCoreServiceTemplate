using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Lykke.Http;
using Lykke.Template.Abstractions;

namespace Lykke.Template.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class SamplesController : Controller
    {
        private readonly ISamplesRepository _samplesRepository;

        public SamplesController(ISamplesRepository samplesRepository)
        {
            _samplesRepository = samplesRepository;
        }

        [HttpPost]
        public Task<IActionResult> InsertAsync([FromBody] Sample model)
        {
            return _samplesRepository
                .InsertAsync(model)
                .ToActionResult();
        }
        
        [HttpPut("{id}")]
        public Task<IActionResult> UpdateAsync(string id, [FromBody] Sample model)
        {
            if (!id.Equals(model.Id))
                return Task.FromException<IActionResult>(
                    new Exception("Invalid data: Wrong Id value."))
                    .ToActionResult();

            return _samplesRepository
                .UpdateAsync(model)
                .ToActionResult();
        }

        [HttpGet("{id}")]
        public Task<IActionResult> GetAsync(string id)
        {
            return _samplesRepository
                .GetAsync(id)
                .ToActionResult();
        }

        [HttpGet]
        public Task<IActionResult> GetAsync()
        {
            return _samplesRepository
                .GetAsync()
                .ToActionResult();
        }
    }

    public static class TaskExtender
    {
        public static async Task<IActionResult> ToActionResult(this Task task)
        {
            try
            {
                await task;
                return new NoContentResult();
            }
            catch (Exception e)
            {
                return GetExeptionResult(e);
            }
        }

        public static async Task<IActionResult> ToActionResult<T>(this Task<T> task)
        {
            try
            {
                var result = await task;
                if (result == null)
                    return new NotFoundResult();

                return new OkObjectResult(result);
            }
            catch (Exception e)
            {
                return GetExeptionResult(e);
            }
        }

        private static IActionResult GetExeptionResult(Exception e)
        {
            return new BadRequestObjectResult(RestException.Map(e));
        }
    }
}