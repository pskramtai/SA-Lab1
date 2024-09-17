using WebApi.Services.Contracts;

namespace WebApi.Services;

public class ProductService : IProductService
{
    private readonly List<Product> _products = [];
    
    public IReadOnlyCollection<Product> GetProductList() => 
        _products;

    public Product? GetProduct(Guid id) => 
        _products.FirstOrDefault(x => x.Id == id);

    public Product AddProduct(Product product)
    {
        var newProduct = product with { Id = Guid.NewGuid() };
        
        _products.Add(newProduct);

        return newProduct;
    }

    public Product? UpdateProduct(Product product)
    {
        var productIndex = _products.FindIndex(x => x.Id == product.Id);

        if (productIndex == -1)
        {
            return null;
        }
        
        _products[productIndex] = product;
            
        return product;

    }

    public void DeleteProduct(Guid id)
    {
        var product = _products.FirstOrDefault(x => x.Id == id);

        if (product is not null)
        { 
            _products.Remove(product);
        }
    }
}