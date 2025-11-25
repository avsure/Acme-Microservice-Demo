using Acme.ProductService.Api.DTOs;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace Acme.ProductService.IntegrationTests
{
    public class ProductApiTests : IClassFixture<TestApplicationFactory>
    {
        private readonly HttpClient _client;

        public ProductApiTests(TestApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Should_Create_Product()
        {
            var request = new CreateProductRequest() { Name = "Keyboard", Price = 999 };

            var response = await _client.PostAsJsonAsync("/api/products", request);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task Should_Get_Product_By_Id()
        {
            // Arrange - Create product first
            var request = new CreateProductRequest { Name = "Mouse", Price = 499 };
            var createResponse = await _client.PostAsJsonAsync("/api/products", request);
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            // Extract created product Id
            var location = createResponse.Headers.Location!.ToString(); // /api/products/{id}
            var id = location.Split("/").Last();

            // Act
            var response = await _client.GetAsync($"/api/products/{id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var product = await response.Content.ReadFromJsonAsync<Product_Dto>();
            product!.Name.Should().Be("Mouse");
        }

        [Fact]
        public async Task Should_Get_All_Products()
        {
            // Arrange
            await _client.PostAsJsonAsync("/api/products", new CreateProductRequest { Name = "Laptop", Price = 90000 });
            await _client.PostAsJsonAsync("/api/products", new CreateProductRequest { Name = "Monitor", Price = 12000 });

            // Act
            var response = await _client.GetAsync("/api/products");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var products = await response.Content.ReadFromJsonAsync<IEnumerable<Product_Dto>>();

            products.Should().NotBeNull();
            products!.Count().Should().BeGreaterThanOrEqualTo(2);
        }
    }
}
