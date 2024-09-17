namespace WebApi.Services.Contracts;

public record Product
(
    Guid Id,
    string Name,
    int Price,
    int Quantity,
    ProductCategory ProductCategory
);
    
public enum ProductCategory
{
    Electronics,
    Clothing,
    Food,
    Other
}