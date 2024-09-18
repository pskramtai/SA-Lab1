using WebApi.Presentation.Models.Request;
using WebApi.Presentation.Models.Response;
using WebApi.Services.Contracts;

namespace WebApi.Presentation;

public static class MapperExtensions
{
    public static ProductDto ToDto(this ProductRequest product) =>
        new
        (
            Id: null,
            Name: product.Name,
            Description: product.Description,
            Price: product.Price,
            Quantity: product.Quantity,
            ProductCategory: product.ProductCategory
        );

    public static ProductResponse ToResponse(this ProductDto product) =>
        new
        (
            Id: product.Id!.Value,
            Name: product.Name,
            Description: product.Description,
            Price: product.Price,
            Quantity: product.Quantity,
            ProductCategory: product.ProductCategory
        );
}