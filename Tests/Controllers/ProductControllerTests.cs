using AutoFixture;
using FluentAssertions;
using Lab1.Controllers;
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
    public void Index_ReturnsAllProducts()
    {
        // Arrange
        
        var products = _fixture
            .CreateMany<Product>()
            .ToList();

        _productServiceMock.Setup(x => x.GetList()).Returns(products);
        
        // Act
        
        var result = _productsController.Index() as ViewResult;
        
        // Assert

        result!.Model.Should().BeEquivalentTo(products);
    }
    
    [Fact]
    public void Edit_ProductExists_ReturnsViewResultWithProduct()
    {
        // Arrange
        
        var product = _fixture.Create<Product>();

        _productServiceMock.Setup(x => x.Get(product.Id)).Returns(product);
        
        // Act
        
        var result = _productsController.Edit(product.Id) as ViewResult;
        
        // Assert

        result!.Model.Should().Be(product);
    }
    
    [Fact]
    public void Edit_ProductDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        
        var guid = _fixture.Create<Guid>();

        _productServiceMock.Setup(x => x.Get(guid)).Returns((Product?) null);
        
        // Act
        
        var result = _productsController.Edit(guid);
        
        // Assert

        result.Should().BeOfType<NotFoundResult>();
    }
    
    [Fact]
    public void Add_InvalidProductModel_ReturnsValidationError()
    {
        // Arrange

        _productsController.ModelState.AddModelError("Name", "Required");

        var product = _fixture.Create<Product>();
        
        // Act

        _productsController.Add(product);

        // Assert

        _productsController.ModelState.Should().ContainKey("Name");
    }
    
    [Fact]
    public void Add_ValidProductModel_RedirectsToEdit()
    {
        // Arrange
        
        var product = _fixture.Create<Product>();
        
        // Act

        var result = _productsController.Add(product) as RedirectToActionResult;
        
        // Assert

        result!.ActionName.Should().Be("Edit");
    }
    
    [Fact]
    public void Update_InvalidProductModel_ReturnsValidationError()
    {
        // Arrange

        _productsController.ModelState.AddModelError("Name", "Required");

        var product = _fixture.Create<Product>();
        
        // Act

        _productsController.Update(product);

        // Assert

        _productsController.ModelState.Should().ContainKey("Name");
    }
    
    [Fact]
    public void Update_ValidProductModel_RedirectsToEdit()
    {
        // Arrange
        
        var product = _fixture.Create<Product>();
        
        // Act

        var result = _productsController.Add(product) as RedirectToActionResult;
        
        // Assert

        result!.ActionName.Should().Be("Edit");
    }
    
    [Fact]
    public void Remove_ValidId_RedirectsToIndex()
    {
        // Arrange
        var productId = Guid.NewGuid();
        _productServiceMock.Setup(service => service.Remove(productId));

        // Act
        var result = _productsController.Remove(productId) as RedirectToActionResult;

        // Assert
        
        result!.ActionName.Should().Be("Index");
    }
}