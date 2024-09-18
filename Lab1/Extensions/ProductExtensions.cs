using Lab1.Models;
using Lab1.Services.Contracts;
using DtoProductCategory = Lab1.Services.Contracts.ProductCategory;
using ProductCategory = Lab1.Models.ProductCategory;

namespace Lab1.Extensions;

public static class ProductExtensions
{
    public static Product ToModel(this ProductDto product) =>
        new(
            Id: product.Id,
            Name: product.Name,
            Description: product.Description,
            Price: product.Price,
            Quantity: product.Quantity,
            ProductCategory: (ProductCategory) product.ProductCategory
        );

    public static ProductDto ToProductDto(this Product product) =>
        new(
            Id: product.Id,
            Name: product.Name,
            Description: product.Description,
            Price: product.Price,
            Quantity: product.Quantity,
            ProductCategory: (DtoProductCategory) product.ProductCategory
        );
}