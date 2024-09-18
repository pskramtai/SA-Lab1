using WebApi.Shared;

namespace WebApi.Presentation.Models.Response;

public record ProductResponse
(
    Guid Id,
    string Name,
    string Description,
    int Price,
    int Quantity,
    ProductCategory ProductCategory
);