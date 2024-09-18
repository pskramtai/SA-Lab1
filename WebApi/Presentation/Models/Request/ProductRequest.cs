using WebApi.Shared;

namespace WebApi.Presentation.Models.Request;

public record ProductRequest
(
    string Name,
    string Description,
    int Price,
    int Quantity,
    ProductCategory ProductCategory
);