using Lab1.Models;
using Lab1.Services.Contracts;

namespace Lab1.Services;

public class ProductService : IProductService
{
    private readonly List<Product> _products = new();
    
    public IReadOnlyCollection<Product> GetList()
    {
        return _products;
    }

    public Product? Get(Guid guid) => 
        _products.FirstOrDefault(x => x.Id == guid);

    public void Add(Product product) =>
        _products.Add(product);

    public void Update(Product product)
    {
        var productIndex = _products.FindIndex(x => x.Id == product.Id);

        if (productIndex != -1)
        {
            _products[productIndex] = product;
        }
    }

    public void Remove(Guid guid)
    {
        var product = _products.FirstOrDefault(x => x.Id == guid);

        if (product is not null)
        { 
            _products.Remove(product);
        }
    }
}