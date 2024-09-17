using WebApi.Shared;

namespace WebApi.Persistence.Entities;

public record ProductEntity
(
    Guid? Id,
    string Name,
    int Price,
    int Quantity,
    ProductCategory ProductCategory
);