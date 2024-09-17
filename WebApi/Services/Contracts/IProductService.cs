namespace WebApi.Services.Contracts;

public interface IProductService
{
    IReadOnlyCollection<Product> GetProductList();
    
    Product? GetProduct(Guid id);
    
    Product AddProduct(Product product);
    
    Product? UpdateProduct(Product product);
    
    void DeleteProduct(Guid id);
}