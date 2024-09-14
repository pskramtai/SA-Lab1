using Lab1.Models;

namespace Lab1.Services.Contracts;

public interface IProductService
{
    IReadOnlyCollection<Product> GetList();

    Product? Get(Guid guid);
    
    void Add(Product product);
    
    void Update(Product product);
    
    void Remove(Guid guid);
}