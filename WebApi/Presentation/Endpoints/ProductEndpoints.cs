using WebApi.Presentation.Filters;
using WebApi.Presentation.Models.Request;
using WebApi.Services.Contracts;

namespace WebApi.Presentation.Endpoints;

public static class ProductEndpoints
{
    public static void RegisterProductRoutes(this WebApplication app)
    {
        app.MapGet("/products", async (IProductService productService) =>
            {
                var productList = await productService.GetProductList();

                return productList.Select(x => x.ToResponse()).ToList();
            })
            .WithName("GetProducts")
            .WithOpenApi();

        app.MapGet("/products/{id}", async (IProductService productService, Guid id) =>
            {
                var product = await productService.GetProduct(id);

                return product is not null ? Results.Ok(product.ToResponse()) : Results.NotFound();
            })
            .WithName("GetProduct")
            .Produces<ProductDto>()
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi();

        app.MapPost("/products", async (IProductService productService, ProductRequest product) =>
            {
                var createdProduct = await productService.CreateProduct(product.ToDto());
        
                return createdProduct.ToResponse();
            })
            .AddEndpointFilter<ValidationFilter<ProductRequest>>()
            .WithName("CreateProduct")
            .WithOpenApi();

        app.MapPut("/products/{id}", async (IProductService productService, ProductRequest product, Guid id) =>
            {
                var existingProduct = await productService.GetProduct(id);

                if (existingProduct is null)
                {
                    return Results.NotFound();
                }
                
                var updatedProduct = await productService.UpdateProduct
                (
                    product.ToDto() with { Id = existingProduct.Id }
                );

                return Results.Ok(updatedProduct.ToResponse());
            })
            .AddEndpointFilter<ValidationFilter<ProductRequest>>()
            .WithName("UpdateProduct")
            .WithOpenApi();

        app.MapDelete("/products/{id}", async (IProductService productService, Guid id) =>
            {
                var existingProduct = await productService.GetProduct(id);

                if (existingProduct is null)
                {
                    return Results.NotFound();
                }

                await productService.DeleteProduct(existingProduct);
        
                return Results.NoContent();
            })
            .WithName("DeleteProduct")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi();
    }
}