namespace Lab1.Services.Contracts;

public interface IProductService
{
    Task<IReadOnlyCollection<ProductDto>> GetListAsync();

    Task<ProductDto?> GetAsync(Guid guid);
    
    Task<ProductDto> AddAsync(ProductDto product);
    
    Task<ProductDto?> UpdateAsync(ProductDto product);
    
    Task DeleteAsync(Guid guid);
}