using Microsoft.EntityFrameworkCore;
using WebApi.Persistence.Entities;

namespace WebApi.Persistence;

public class ProductDbContext(DbContextOptions<ProductDbContext> options) : DbContext(options)
{
    public DbSet<ProductEntity> Products { get; init; }
}