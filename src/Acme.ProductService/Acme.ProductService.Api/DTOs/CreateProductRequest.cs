namespace Acme.ProductService.Api.DTOs
{
    public class CreateProductRequest
    {
        public string Name { get; set; } = default!;
        public decimal Price { get; set; }
    }
}
