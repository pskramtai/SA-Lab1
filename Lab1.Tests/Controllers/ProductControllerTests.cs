using AutoFixture;
using FluentAssertions;
using Lab1.Controllers;
using Lab1.Extensions;
using Lab1.Models;
using Lab1.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Tests.Controllers;

public class ProductControllerTests
{
    private readonly Mock<IProductService> _productServiceMock = new();
    private readonly Fixture _fixture = new();

    private readonly ProductController _productsController;

    public ProductControllerTests() => 
        _productsController = new ProductController(_productServiceMock.Object);

    [Fact]
    public async Task Index_ReturnsAllProducts()
    {
        // Arrange
        
        var products = _fixture
            .CreateMany<ProductDto>()
            .ToList();

        _productServiceMock.Setup(x => x.GetListAsync()).ReturnsAsync(products);
        
        // Act
        
        var result = await _productsController.Index() as ViewResult;
        
        // Assert

        result!.Model.Should().BeEquivalentTo(products);
    }
    
    [Fact]
    public async Task Edit_ProductExists_ReturnsViewResultWithProduct()
    {
        // Arrange
        
        var product = _fixture.Create<Product>();
        var productDto = product.ToProductDto();

        _productServiceMock.Setup(x => x.GetAsync(product.Id)).ReturnsAsync(productDto);
        _productServiceMock.Setup(x => x.UpdateAsync(productDto)).ReturnsAsync(productDto);
        
        // Act
        
        var result = await _productsController.Edit(product.Id) as ViewResult;
        
        // Assert

        result!.Model.Should().Be(product);
    }
    
    [Fact]
    public async Task Edit_ProductDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        
        var guid = _fixture.Create<Guid>();

        _productServiceMock.Setup(x => x.GetAsync(guid)).ReturnsAsync((ProductDto?) null);
        
        // Act
        
        var result = await _productsController.Edit(guid);
        
        // Assert

        result.Should().BeOfType<NotFoundResult>();
    }
    
    [Fact]
    public async Task Add_InvalidProductName_ReturnsValidationError()
    {
        // Arrange

        _productsController.ModelState.AddModelError(nameof(Product.Name), "Required");

        var product = _fixture.Create<Product>();
        
        // Act

        await _productsController.Add(product);

        // Assert

        _productsController.ModelState.Should().ContainKey(nameof(Product.Name));
    }
    
    [Fact]
    public async Task Add_InvalidProductDescription_ReturnsValidationError()
    {
        // Arrange

        _productsController.ModelState.AddModelError(nameof(Product.Description), "Required");

        var product = _fixture.Create<Product>();
        
        // Act

        await _productsController.Add(product);

        // Assert

        _productsController.ModelState.Should().ContainKey(nameof(Product.Description));
    }
    
    [Fact]
    public async Task Add_InvalidProductPrice_ReturnsValidationError()
    {
        // Arrange

        _productsController.ModelState.AddModelError(nameof(Product.Price), "Required");

        var product = _fixture.Create<Product>();
        
        // Act

        await _productsController.Add(product);

        // Assert

        _productsController.ModelState.Should().ContainKey(nameof(Product.Price));
    }
    
    [Fact]
    public async Task Add_InvalidProductQuantity_ReturnsValidationError()
    {
        // Arrange

        _productsController.ModelState.AddModelError(nameof(Product.Quantity), "Required");

        var product = _fixture.Create<Product>();
        
        // Act

        await _productsController.Add(product);

        // Assert

        _productsController.ModelState.Should().ContainKey(nameof(Product.Quantity));
    }
    
    [Fact]
    public async Task Add_InvalidProductCategory_ReturnsValidationError()
    {
        // Arrange

        _productsController.ModelState.AddModelError(nameof(Product.ProductCategory), "Required");

        var product = _fixture.Create<Product>();
        
        // Act

        await _productsController.Add(product);

        // Assert

        _productsController.ModelState.Should().ContainKey(nameof(Product.ProductCategory));
    }
    
    [Fact]
    public async Task Add_ValidProductModel_RedirectsToEdit()
    {
        // Arrange
        
        var product = _fixture.Create<Product>();
        var productDto = product.ToProductDto();

        _productServiceMock.Setup(x => x.AddAsync(productDto)).ReturnsAsync(productDto);
        
        // Act

        var result = await _productsController.Add(product) as RedirectToActionResult;
        
        // Assert

        result!.ActionName.Should().Be("Edit");
    }
    
    [Fact]
    public async Task Update_ValidProductModel_RedirectsToEdit()
    {
        // Arrange
        
        var product = _fixture.Create<Product>();
        var productDto = product.ToProductDto();

        _productServiceMock.Setup(x => x.UpdateAsync(productDto)).ReturnsAsync(productDto);
        
        
        // Act

        var result = await _productsController.Update(product) as RedirectToActionResult;
        
        // Assert

        result!.ActionName.Should().Be("Edit");
    }
    
    [Fact]
    public async Task Remove_ValidId_RedirectsToIndex()
    {
        // Arrange
        var productId = Guid.NewGuid();
        _productServiceMock.Setup(service => service.DeleteAsync(productId));

        // Act
        var result = await _productsController.Remove(productId) as RedirectToActionResult;

        // Assert
        
        result!.ActionName.Should().Be("Index");
    }
}