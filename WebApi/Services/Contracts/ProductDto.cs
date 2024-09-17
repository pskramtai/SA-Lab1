using WebApi.Shared;

namespace WebApi.Services.Contracts;

public record ProductDto
(
    Guid? Id,
    string Name,
    int Price,
    int Quantity,
    ProductCategory ProductCategory
);