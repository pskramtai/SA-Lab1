using WebApi.Persistence.Entities;
using WebApi.Services.Contracts;

namespace WebApi.Services;

public static class MapperExtensions
{
    public static ProductEntity ToEntity(this ProductDto product) =>
        new
        (
            Id: product.Id,
            Name: product.Name,
            Description: product.Description,
            Price: product.Price,
            Quantity: product.Quantity,
            ProductCategory: product.ProductCategory
        );

    public static ProductDto ToDto(this ProductEntity product) =>
        new
        (
            Id: product.Id,
            Price: product.Price,
            Description: product.Description,
            Name: product.Name,
            Quantity: product.Quantity,
            ProductCategory: product.ProductCategory
        );
}