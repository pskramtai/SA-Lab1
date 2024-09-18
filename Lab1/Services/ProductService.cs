using Lab1.Services.Clients;
using Lab1.Services.Contracts;

namespace Lab1.Services;

public class ProductService(IProductApiClient client) : IProductService
{
    public async Task<IReadOnlyCollection<ProductDto>> GetListAsync() => 
        await client.GetProductsAsync();

    public async Task<ProductDto?> GetAsync(Guid guid) =>
        await client.GetProductAsync(guid);

    public async Task<ProductDto> AddAsync(ProductDto product) => 
        await client.CreateProductAsync(product);

    public async Task<ProductDto?> UpdateAsync(ProductDto product) => 
        await client.UpdateProductAsync(product.Id, product);

    public async Task DeleteAsync(Guid guid) => 
        await client.DeleteProductAsync(guid);
}