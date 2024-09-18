using WebApi.Persistence.Repositories;
using WebApi.Services.Contracts;

namespace WebApi.Services;

public class ProductService(IProductRepository repository) : IProductService
{
    public async Task<IReadOnlyCollection<ProductDto>> GetProductList() => 
        (await repository.GetAllAsync()).Select(x => x.ToDto()).ToList();

    public async Task<ProductDto?> GetProduct(Guid id) =>
        (await repository.GetByIdAsync(id))?.ToDto();

    public async Task<ProductDto> AddProduct(ProductDto productDto)
    {
        var productEntity = productDto.ToEntity();
        
        var newProduct = await repository.AddAsync(productEntity);

        return newProduct.ToDto();
    }

    public async Task<ProductDto> UpdateProduct(ProductDto productDto)
    {
        var productEntity = productDto.ToEntity();
        
        var updatedProduct = await repository.UpdateAsync(productEntity);

        return updatedProduct.ToDto();
    }

    public async Task DeleteProduct(ProductDto productDto) => 
        await repository.DeleteAsync(productDto.ToEntity());
}