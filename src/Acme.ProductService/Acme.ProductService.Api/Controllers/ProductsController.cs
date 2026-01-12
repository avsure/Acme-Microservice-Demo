using Acme.Contracts;
using Acme.ProductService.Api.DTOs;
using Acme.ProductService.Application.DTOs;
using Acme.ProductService.Application.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Acme.ProductService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<ProductsController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductsController(IProductService productService,
            IPublishEndpoint publishEndpoint,
            ILogger<ProductsController> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _productService = productService;
            _publishEndpoint = publishEndpoint;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            Console.WriteLine("*********CONSOLE TEST LOG*********");
            Serilog.Log.Information("*********Serilog static test log*********");
            _logger.LogInformation("*********Getting all the Products*********");

            var product = await _productService.GetAllProductsAsync();
            if (product == null) return NotFound();

            List<Product_Dto> prodList = new List<Product_Dto>();
            foreach (ProductDto item in product)
            {
                Product_Dto product_Dto = new Product_Dto(item.Id, item.Name, item.Price);
                prodList.Add(product_Dto);
            }
            _logger.LogInformation(_logger.IsEnabled(LogLevel.Information)
                ? "Retrieved products: {Products}"
                : "Retrieved products", string.Join(", ", prodList.Select(p => p.Name)));
            _logger.LogInformation("Returning {Count} products", prodList.Count);

            return Ok(prodList);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();

            _logger.LogInformation("Retrieved product: {ProductName} with ID: {ProductId}", product.Name, product.Id);

            return Ok(new Product_Dto(product.Id, product.Name, product.Price));
        }

        [HttpPost]
        [Consumes("application/json")]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
        {
            _logger.LogInformation("POST /api/products reached");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var correlationId = _httpContextAccessor.HttpContext?
                .Items["X-Correlation-ID"]?
                .ToString();

            _logger.LogInformation("X-Correlation-ID : "+ correlationId);

            var id = await _productService.CreateProductAsync(
                new ProductCreateModel(request.Name, request.Price, string.Empty, string.Empty)
            );

            if (_publishEndpoint != null)
            {
                // 🔥 Fire-and-forget publish (DO NOT BLOCK HTTP RESPONSE)
                _ = _publishEndpoint.Publish<IProductCreated>(new
                {
                    ProductId = id,
                    Name = request.Name,
                    Price = request.Price,
                    CreatedAt = DateTime.UtcNow
                },
                context =>
                {
                    if (!string.IsNullOrEmpty(correlationId))
                    {
                    context.Headers.Set("X-Correlation-ID", correlationId);
                    }
                });
            }

            _logger.LogInformation(
                "Product created. ProductId: {ProductId}, CorrelationId: {CorrelationId}",
                id, correlationId);

            return StatusCode(StatusCodes.Status201Created);
        }
    }
}
