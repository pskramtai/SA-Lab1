namespace WebApi.Services.Contracts;

public interface IProductService
{
    Task<IReadOnlyCollection<ProductDto>> GetProductList();
    
    Task<ProductDto?> GetProduct(Guid id);
    
    Task<ProductDto> CreateProduct(ProductDto productDto);
    
    Task<ProductDto> UpdateProduct(ProductDto productDto);
    
    Task DeleteProduct(ProductDto productDto);
}