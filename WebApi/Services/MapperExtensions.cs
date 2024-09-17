using WebApi.Persistence.Entities;
using WebApi.Services.Contracts;

namespace WebApi.Services;

public static class MapperExtensions
{
    public static ProductEntity ToEntity(this ProductDto productDto) =>
        new
        (
            Id: productDto.Id,
            Name: productDto.Name,
            Price: productDto.Price,
            Quantity: productDto.Quantity,
            ProductCategory: productDto.ProductCategory
        );

    public static ProductDto ToDto(this ProductEntity productEntity) =>
        new
        (
            Id: productEntity.Id,
            Price: productEntity.Price,
            Name: productEntity.Name,
            Quantity: productEntity.Quantity,
            ProductCategory: productEntity.ProductCategory
        );
}