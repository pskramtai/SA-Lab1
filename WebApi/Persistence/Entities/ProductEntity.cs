using WebApi.Shared;

namespace WebApi.Persistence.Entities;

public record ProductEntity
(
    Guid? Id,
    string Name,
    string Description,
    int Price,
    int Quantity,
    ProductCategory ProductCategory
);