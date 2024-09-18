using Lab1.Services.Contracts;
using Refit;

namespace Lab1.Services.Clients;

public interface IProductApiClient
{
    [Get("/products")]
    Task<List<ProductDto>> GetProductsAsync();

    [Get("/products/{id}")]
    Task<ProductDto> GetProductAsync(Guid id);

    [Post("/products")]
    Task<ProductDto> CreateProductAsync([Body] ProductDto productRequest);

    [Put("/products/{id}")]
    Task<ProductDto> UpdateProductAsync(Guid id, [Body] ProductDto productRequest);

    [Delete("/products/{id}")]
    Task DeleteProductAsync(Guid id);
}