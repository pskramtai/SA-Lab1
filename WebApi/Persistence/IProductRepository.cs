using WebApi.Persistence.Entities;

namespace WebApi.Persistence;

public interface IProductRepository
{
    Task<ProductEntity?> GetByIdAsync(Guid id);
    
    Task<IEnumerable<ProductEntity>> GetAllAsync();
    
    Task<ProductEntity> AddAsync(ProductEntity product);
    
    Task<ProductEntity> UpdateAsync(ProductEntity product);
    
    Task DeleteAsync(ProductEntity product);
}