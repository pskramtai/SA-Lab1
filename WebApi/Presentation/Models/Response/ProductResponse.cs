using WebApi.Shared;

namespace WebApi.Presentation.Models.Response;

public record ProductResponse
(
    Guid Id,
    string Name,
    int Price,
    int Quantity,
    ProductCategory ProductCategory
);