using Acme.Contracts;
using Acme.ProductService.Api.DTOs;
using Acme.ProductService.Application.DTOs;
using Acme.ProductService.Application.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Acme.ProductService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IPublishEndpoint _publishEndpoint;
        public ProductsController(IProductService productService, IPublishEndpoint publishEndpoint)
        {
            _productService = productService;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var product = await _productService.GetAllProductsAsync();
            if (product == null) return NotFound();

            List<Product_Dto> prodList = new List<Product_Dto>();
            foreach (ProductDto item in product)
            {
               Product_Dto product_Dto = new Product_Dto(item.Id, item.Name, item.Price);
                prodList.Add(product_Dto);
            }
            return Ok(prodList);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();

            return Ok(new Product_Dto(product.Id, product.Name, product.Price));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductRequest request)
        {
            var id = await _productService.CreateProductAsync(
                    new ProductCreateModel(request.Name, request.Price,string.Empty,string.Empty)
                 );

            // get domain/model data directly
            var product = await _productService.GetProductByIdAsync(id);

            await _publishEndpoint.Publish<IProductCreated>(new
            {
                ProductId = product.Id,
                Name = product.Name,
                Price = product.Price,
                CreatedAt = DateTime.UtcNow
            });

            return CreatedAtAction(nameof(Get), new { id }, null);
        }
    }
}
