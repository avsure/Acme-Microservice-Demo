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
        private readonly ILogger<RecommendationsController> _logger;
        public RecommendationsController(IRecommendationServices service, ILogger<RecommendationsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetForProduct(Guid productId)
        {
            _logger.LogInformation("Recommendation: Get Product By Id test log");

            Serilog.Log.Information("Serilog static test log");

            var result = await _service.GetForProductAsync(productId);

            _logger.LogInformation("Retrieved {Count} recommendations for product {ProductId}", result.Count(), productId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Guid productId, string message)
        {
            var id = await _service.CreateRecommendationAsync(productId, message);

            _logger.LogInformation("Created recommendation {RecommendationId} for product {ProductId}", id, productId);

            return Ok(new { id });
        }
    }
}
