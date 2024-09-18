namespace Lab1.Services.Contracts;

public record ProductDto
(
    Guid Id,
    string Name,
    string Description,
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