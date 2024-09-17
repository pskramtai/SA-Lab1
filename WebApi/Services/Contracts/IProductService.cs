namespace WebApi.Services.Contracts;

public interface IProductService
{
    Task<IReadOnlyCollection<ProductDto>> GetProductList();
    
    Task<ProductDto?> GetProduct(Guid id);
    
    Task<ProductDto> AddProduct(ProductDto productDto);
    
    Task<ProductDto> UpdateProduct(ProductDto productDto);
    
    Task DeleteProduct(ProductDto productDto);
}