using System.Text.Json;
using System.Text.Json.Serialization;
using Lab1.Services;
using Lab1.Services.Clients;
using Lab1.Services.Contracts;
using Refit;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddScoped<IProductService, ProductService>()
    .AddControllersWithViews();

builder.Services.AddRefitClient<IProductApiClient>(x =>
    {
        return new RefitSettings
        {
            ContentSerializer = new SystemTextJsonContentSerializer(GetSerializerOptions()),

        };

        JsonSerializerOptions GetSerializerOptions()
        {
            var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            jsonOptions.Converters.Add(new JsonStringEnumConverter());

            return jsonOptions;
        }
    })
    .ConfigureHttpClient(client =>
    {
        client.BaseAddress = new Uri(builder.Configuration["ProductApiBaseUrl"]!);
    });
    
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();