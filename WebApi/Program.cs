using System.Text.Json.Serialization;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WebApi.Persistence;
using WebApi.Persistence.Repositories;
using WebApi.Presentation;
using WebApi.Presentation.Filters;
using WebApi.Presentation.Validators;
using WebApi.Services;
using WebApi.Services.Contracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(options =>
    {
        options.SchemaFilter<CustomSchemaFilter>();
    })
    .AddScoped<IProductService, ProductService>()
    .AddScoped<IProductRepository, ProductRepository>()
    .AddValidatorsFromAssemblyContaining<ProductRequestValidator>();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureExceptionHandling();

app.UseHttpsRedirection();

app.RegisterRoutes();

app.Run();