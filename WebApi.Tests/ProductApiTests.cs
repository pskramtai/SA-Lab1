using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WebApi.Presentation.Models.Request;
using WebApi.Presentation.Models.Response;
using WebApi.Services.Contracts;
using WebApi.Shared;

namespace WebApi.Tests;

public class ProductsApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly Mock<IProductService> _productServiceMock;
    private readonly Fixture _fixture = new();

    private readonly HttpClient _sut;
    
    public ProductsApiTests(WebApplicationFactory<Program> factory)
    {
        _productServiceMock = new Mock<IProductService>();
        
        _sut = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddScoped(_ => _productServiceMock.Object);
                services.ConfigureHttpJsonOptions(options =>
                {
                    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });
            });
        }).CreateClient();
    }

    #region GetProducts Endpoint Tests

    [Fact]
    public async Task GetProducts_WhenProductsExist_ShouldReturnProductList()
    {
        // Arrange

        var products = _fixture.CreateMany<ProductDto>().ToList();
        
        _productServiceMock.Setup(s => s.GetProductList()).ReturnsAsync(products);
        
        // Act
        
        var response = await _sut.GetAsync("/products");

        // Assert
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var productList = await ParseResponse<List<ProductResponse>>(response);
        productList.Should().BeEquivalentTo(products);
    }
    
    [Fact]
    public async Task GetProducts_WhenProductsDontExist_ShouldReturnEmptyProductList()
    {
        // Arrange

        var products = new List<ProductDto>();
        
        _productServiceMock.Setup(s => s.GetProductList()).ReturnsAsync(products);
        
        // Act
        
        var response = await _sut.GetAsync("/products");

        // Assert
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var productList = await ParseResponse<List<ProductResponse>>(response);
        productList.Should().BeEmpty();
    }

    #endregion

    #region GetProduct Endpoint Tests

    [Fact]
    public async Task GetProduct_WhenExists_ShouldReturnProduct()
    {
        // Arrange
        
        var productId = Guid.NewGuid();
        
        var product = _fixture
            .Build<ProductDto>()
            .With(x => x.Id, productId)
            .Create();
        
        _productServiceMock.Setup(s => s.GetProduct(productId)).ReturnsAsync(product);
        
        // Act
        
        var response = await _sut.GetAsync($"/products/{productId}");

        // Assert
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var result = await ParseResponse<ProductResponse>(response);
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(product);
    }

    [Fact]
    public async Task GetProduct_WhenProductDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        
        var productId = Guid.NewGuid();
        
        _productServiceMock.Setup(s => s.GetProduct(productId)).ReturnsAsync((ProductDto?)null);
        
        // Act
        
        var response = await _sut.GetAsync($"/products/{productId}");

        // Assert
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion

    #region CreateProduct Endpoint Tests

    [Fact]
    public async Task CreateProduct_WhenRequestValid_ShouldReturnCreatedProduct()
    {
        // Arrange
        
        var productRequest = _fixture
            .Build<ProductRequest>()
            .With(x => x.Name, "test")
            .With(x => x.Description, "test")
            .With(x => x.Price, 1)
            .With(x => x.Quantity, 1)
            .With(x => x.ProductCategory, ProductCategory.Other)
            .Create();
        
        var newProductId = Guid.NewGuid();
        
        var productDto = _fixture
            .Build<ProductDto>()
            .With(x => x.Id, (Guid?) null)
            .With(x => x.Name, productRequest.Name)
            .With(x => x.Description, productRequest.Description)
            .With(x => x.Price, productRequest.Price)
            .With(x => x.Quantity, productRequest.Quantity)
            .With(x => x.ProductCategory, productRequest.ProductCategory)
            .Create();

        var createdProduct = productDto with { Id = newProductId };

        _productServiceMock.Setup(s => s.CreateProduct(productDto))
            .ReturnsAsync(createdProduct);

        // Act
        
        var response = await _sut.PostAsJsonAsync("/products", productRequest);

        // Assert
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var result = await ParseResponse<ProductResponse>(response);
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(createdProduct);
    }

    [Fact]
    public async Task CreateProduct_WhenNameInvalid_ShouldReturnBadRequest()
    {
        // Arrange
        
        var productRequest = _fixture
            .Build<ProductRequest>()
            .With(x => x.Name, "12")
            .Create();

        // Act
        
        var response = await _sut.PostAsJsonAsync("/products", productRequest);

        // Assert
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        _productServiceMock.Verify(x => x.CreateProduct(It.IsAny<ProductDto>()), Times.Never);
    }
    
    [Fact]
    public async Task CreateProduct_WhenDescriptionInvalid_ShouldReturnBadRequest()
    {
        // Arrange
        
        var productRequest = _fixture
            .Build<ProductRequest>()
            .With(x => x.Description, "12")
            .Create();

        // Act
        
        var response = await _sut.PostAsJsonAsync("/products", productRequest);

        // Assert
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        _productServiceMock.Verify(x => x.CreateProduct(It.IsAny<ProductDto>()), Times.Never);
    }
    
    [Fact]
    public async Task CreateProduct_WhenPriceInvalid_ShouldReturnBadRequest()
    {
        // Arrange
        
        var productRequest = _fixture
            .Build<ProductRequest>()
            .With(x => x.Price, -1)
            .Create();

        // Act
        
        var response = await _sut.PostAsJsonAsync("/products", productRequest);

        // Assert
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        _productServiceMock.Verify(x => x.CreateProduct(It.IsAny<ProductDto>()), Times.Never);
    }
    
    [Fact]
    public async Task CreateProduct_WhenQuantityInvalid_ShouldReturnBadRequest()
    {
        // Arrange
        
        var productRequest = _fixture
            .Build<ProductRequest>()
            .With(x => x.Quantity, -1)
            .Create();

        // Act
        
        var response = await _sut.PostAsJsonAsync("/products", productRequest);

        // Assert
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        _productServiceMock.Verify(x => x.CreateProduct(It.IsAny<ProductDto>()), Times.Never);
    }
    
    [Fact]
    public async Task CreateProduct_WhenProductCategoryInvalid_ShouldReturnBadRequest()
    {
        // Arrange
        
        var productRequest = _fixture
            .Build<ProductRequest>()
            .With(x => x.ProductCategory, (ProductCategory)10)
            .Create();

        // Act
        
        var response = await _sut.PostAsJsonAsync("/products", productRequest);

        // Assert
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        _productServiceMock.Verify(x => x.CreateProduct(It.IsAny<ProductDto>()), Times.Never);
    }
    
    #endregion

    #region UpdateProduct Endpoint Tests

    [Fact]
    public async Task UpdateProduct_WhenProductExistsAndRequestValid_ShouldReturnUpdatedProduct()
    {
        // Arrange
        
        var productRequest = _fixture
            .Build<ProductRequest>()
            .With(x => x.Name, "test")
            .With(x => x.Description, "test")
            .With(x => x.Price, 1)
            .With(x => x.Quantity, 1)
            .With(x => x.ProductCategory, ProductCategory.Other)
            .Create();
        
        var productId = Guid.NewGuid();
        
        var productDto = _fixture
            .Build<ProductDto>()
            .With(x => x.Id, productId)
            .Create();
        
        var updatedProduct = _fixture
            .Build<ProductDto>()
            .With(x => x.Id, productId)
            .With(x => x.Name, productRequest.Name)
            .With(x => x.Description, productRequest.Description)
            .With(x => x.Price, productRequest.Price)
            .With(x => x.Quantity, productRequest.Quantity)
            .With(x => x.ProductCategory, productRequest.ProductCategory)
            .Create();

        _productServiceMock.Setup(s => s.GetProduct(productId)).ReturnsAsync(productDto);
        _productServiceMock.Setup(s => s.UpdateProduct(updatedProduct)).ReturnsAsync(updatedProduct);
        
        // Act
        
        var response = await _sut.PutAsJsonAsync($"/products/{productId}", productRequest);

        // Assert
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await ParseResponse<ProductResponse>(response);
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(updatedProduct);
    }

    [Fact]
    public async Task UpdateProduct_WhenProductDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange

        var productRequest = _fixture
            .Build<ProductRequest>()
            .With(x => x.Name, "test")
            .With(x => x.Description, "test")
            .With(x => x.Price, 1)
            .With(x => x.Quantity, 1)
            .With(x => x.ProductCategory, ProductCategory.Other)
            .Create();
        
        var productId = Guid.NewGuid();
        
        // Act
        
        var response = await _sut.PutAsJsonAsync($"/products/{productId}", productRequest);

        // Assert
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        _productServiceMock.Verify(s => s.GetProduct(productId), Times.Once);
        _productServiceMock.Verify(x => x.UpdateProduct(It.IsAny<ProductDto>()), Times.Never);
    }

    [Fact]
    public async Task UpdateProduct_WhenNameInvalid_ShouldReturnBadRequest()
    {
        // Arrange
        
        var productRequest = _fixture
            .Build<ProductRequest>()
            .With(x => x.Name, "12")
            .Create();
        
        var productId = Guid.NewGuid();
        
        // Act
        
        var response = await _sut.PutAsJsonAsync($"/products/{productId}", productRequest);

        // Assert
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        _productServiceMock.Verify(x => x.UpdateProduct(It.IsAny<ProductDto>()), Times.Never);
    }
    
    [Fact]
    public async Task UpdateProduct_WhenDescriptionInvalid_ShouldReturnBadRequest()
    {
        // Arrange
        
        var productRequest = _fixture
            .Build<ProductRequest>()
            .With(x => x.Description, "12")
            .Create();
        
        var productId = Guid.NewGuid();
        
        // Act
        
        var response = await _sut.PutAsJsonAsync($"/products/{productId}", productRequest);

        // Assert
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        _productServiceMock.Verify(x => x.UpdateProduct(It.IsAny<ProductDto>()), Times.Never);
    }
    
    [Fact]
    public async Task UpdateProduct_WhenPriceInvalid_ShouldReturnBadRequest()
    {
        // Arrange
        
        var productRequest = _fixture
            .Build<ProductRequest>()
            .With(x => x.Price, -1)
            .Create();
        
        var productId = Guid.NewGuid();
        
        // Act
        
        var response = await _sut.PutAsJsonAsync($"/products/{productId}", productRequest);

        // Assert
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        _productServiceMock.Verify(x => x.UpdateProduct(It.IsAny<ProductDto>()), Times.Never);
    }
    
    [Fact]
    public async Task UpdateProduct_WhenQuantityInvalid_ShouldReturnBadRequest()
    {
        // Arrange
        
        var productRequest = _fixture
            .Build<ProductRequest>()
            .With(x => x.Quantity, -1)
            .Create();
        
        var productId = Guid.NewGuid();
        
        // Act
        
        var response = await _sut.PutAsJsonAsync($"/products/{productId}", productRequest);

        // Assert
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        _productServiceMock.Verify(x => x.UpdateProduct(It.IsAny<ProductDto>()), Times.Never);
    }
    
    [Fact]
    public async Task UpdateProduct_WhenProductCategoryInvalid_ShouldReturnBadRequest()
    {
        // Arrange
        
        var productRequest = _fixture
            .Build<ProductRequest>()
            .With(x => x.ProductCategory, (ProductCategory) 10)
            .Create();
        
        var productId = Guid.NewGuid();
        
        // Act
        
        var response = await _sut.PutAsJsonAsync($"/products/{productId}", productRequest);

        // Assert
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        _productServiceMock.Verify(x => x.UpdateProduct(It.IsAny<ProductDto>()), Times.Never);
    }
    
    #endregion

    #region DeleteProduct Endpoint Tests

    [Fact]
    public async Task DeleteProduct_WhenProductExists_ShouldReturnNoContent()
    {
        // Arrange
        
        var productId = Guid.NewGuid();
        var product = _fixture.Create<ProductDto>();

        _productServiceMock.Setup(s => s.GetProduct(productId)).ReturnsAsync(product);
        _productServiceMock.Setup(s => s.DeleteProduct(product)).Returns(Task.CompletedTask);
        
        // Act
        
        var response = await _sut.DeleteAsync($"/products/{productId}");

        // Assert
        
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task DeleteProduct_WhenProductDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        
        var productId = Guid.NewGuid();

        _productServiceMock.Setup(s => s.GetProduct(productId)).ReturnsAsync((ProductDto?) null);
        
        // Act
        
        var response = await _sut.DeleteAsync($"/products/{productId}");

        // Assert
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        _productServiceMock.Verify(x => x.DeleteProduct(It.IsAny<ProductDto>()), Times.Never);
    }

    #endregion

    private static async Task<T?> ParseResponse<T>(HttpResponseMessage response)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        };
        
        return await response.Content.ReadFromJsonAsync<T>(options);
    }
}