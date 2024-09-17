using System.Text.Json.Serialization;
using WebApi.Services;
using WebApi.Services.Contracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IProductService, ProductService>();
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/products", (IProductService productService) => productService.GetProductList())
    .WithName("GetProducts")
    .WithOpenApi();

app.MapGet("/products/{id}", (IProductService productService, Guid id) =>
{
    var product = productService.GetProduct(id);

    return product is not null ? Results.Ok(product) : Results.NotFound();
})
    .WithName("GetProduct")
    .Produces<Product>()
    .Produces(StatusCodes.Status404NotFound)
    .WithOpenApi();

app.MapPost("/products", (IProductService productService, Product product) => productService.AddProduct(product))
    .WithName("CreateProduct")
    .WithOpenApi();

app.MapPut("/products/{id}", (IProductService productService, Product product, Guid id) => productService.UpdateProduct(product))
    .WithName("UpdateProduct")
    .WithOpenApi();

app.MapDelete("/products/{id}", (IProductService productService, Guid id) => productService.DeleteProduct(id))
    .WithName("DeleteProduct")
    .WithOpenApi();

app.Run();