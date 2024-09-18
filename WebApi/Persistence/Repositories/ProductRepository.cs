using Microsoft.EntityFrameworkCore;
using WebApi.Persistence.Entities;

namespace WebApi.Persistence.Repositories;

public class ProductRepository(ProductDbContext context) : IProductRepository
{
    public async Task<ProductEntity?> GetByIdAsync(Guid id) => 
        await context.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

    public async Task<IEnumerable<ProductEntity>> GetAllAsync() => 
        await context.Products.AsNoTracking().ToListAsync();

    public async Task<ProductEntity> AddAsync(ProductEntity product)
    {
        await context.Products.AddAsync(product);
        await context.SaveChangesAsync();

        return product;
    }

    public async Task<ProductEntity> UpdateAsync(ProductEntity product)
    {
        context.Products.Update(product);
        await context.SaveChangesAsync();
        
        return product;
    }

    public async Task DeleteAsync(ProductEntity product)
    {
        context.Products.Remove(product);
        await context.SaveChangesAsync();
    }
}