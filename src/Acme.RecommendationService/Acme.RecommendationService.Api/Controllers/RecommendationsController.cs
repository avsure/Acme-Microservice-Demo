using Acme.RecommendationService.Application.Interfaces;
using Acme.RecommendationService.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Acme.RecommendationService.Api.Controllers
{
    [ApiController]
    [Route("api/recommendations")]
    public class RecommendationsController : ControllerBase
    {
        private readonly IRecommendationServices _service;

        public RecommendationsController(IRecommendationServices service)
        {
            _service = service;
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetForProduct(Guid productId)
        {
            var result = await _service.GetForProductAsync(productId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Guid productId, string message)
        {
            var id = await _service.CreateRecommendationAsync(productId, message);
            return Ok(new { id });
        }
    }
}
