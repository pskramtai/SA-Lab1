using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebApi.Presentation.Models.Request;
using WebApi.Presentation.Models.Response;

namespace WebApi.Presentation.Filters;

public class CustomSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type != typeof(ProductRequest) || context.Type != typeof(ProductResponse)) return;

        schema.Properties["name"].Nullable = false;
        schema.Required.Add("name");
        
        schema.Properties["price"].Nullable = false;
        schema.Required.Add("price");
        
        schema.Properties["quantity"].Nullable = false;
        schema.Required.Add("quantity");
        
        schema.Properties["productCategory"].Nullable = false;
        schema.Required.Add("productCategory");
    }
}