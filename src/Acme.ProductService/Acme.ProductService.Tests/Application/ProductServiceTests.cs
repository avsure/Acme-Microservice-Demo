using Acme.ProductService.Application.DTOs;
using Acme.ProductService.Application.Interfaces;
using Acme.ProductService.Application.Services;
using Acme.ProductService.Domain;
using FluentAssertions;
using Moq;

namespace Acme.ProductService.Tests.Application
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _repoMock;
        private readonly IProductService _productService;

        public ProductServiceTests()
        {
            _repoMock = new Mock<IProductRepository>();
            _productService = new ProductsService(_repoMock.Object);
        }

        [Fact]
        public async Task CreateProductAsync_Should_Add_Product_And_Return_Id()
        {
            // Act
            var id = await _productService.CreateProductAsync(new ProductCreateModel("Laptop",15000,
                "Electronics", "High-end gaming laptop"));

            // Assert
            id.Should().NotBeEmpty();
            _repoMock.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async Task GetProductByIdAsync_Should_Return_ProductDto_When_Exists()
        {
            // Arrange
            var product = new Product(Guid.NewGuid(),"Test Item","Test Category", 99,"Test Description");
            _repoMock.Setup(r => r.GetByIdAsync(product.Id))
                     .ReturnsAsync(product);

            // Act
            var result = await _productService.GetProductByIdAsync(product.Id);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(product.Id);
            result.Name.Should().Be("Test Item");
        }

        [Fact]
        public async Task GetProductByIdAsync_Should_Return_Null_When_Not_Found()
        {
            // Arrange
            _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                     .ReturnsAsync((Product?)null);

            // Act
            var result = await _productService.GetProductByIdAsync(Guid.NewGuid());

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAllProductsAsync_Should_Return_All_Products()
        {
            // Arrange
            var products = new List<Product>
            {
                new(Guid.NewGuid(),"Test Item1","Test Category1", 99,"Test Description1"),
                new(Guid.NewGuid(),"Test Item2","Test Category2", 99,"Test Description2")
            };

            _repoMock.Setup(r => r.GetAllAsync())
                     .ReturnsAsync(products);

            // Act
            var result = await _productService.GetAllProductsAsync();

            // Assert
            result.Should().HaveCount(2);
            result.Select(x => x.Name).Should().Contain(new[] { "Test Item1", "Test Item2"});
        }
    }
}
