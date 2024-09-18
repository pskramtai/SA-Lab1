using AutoFixture;
using FluentAssertions;
using Lab1.Models;
using Lab1.Services;
using Lab1.Services.Clients;
using Lab1.Services.Contracts;
using Moq;
using Xunit;

namespace Tests.Services;

public class ProductServiceTests
    {
        private readonly Mock<IProductApiClient> _productApiClientMock = new();
        private readonly Fixture _fixture = new();

        private readonly ProductService _productService;

        public ProductServiceTests() => 
            _productService = new ProductService(_productApiClientMock.Object);

        [Fact]
        public async Task GetListAsync_EmptyList_ReturnsEmptyCollection()
        {
            // Arrange

            var products = new List<ProductDto>();
            _productApiClientMock.Setup(x => x.GetProductsAsync()).ReturnsAsync(products);
            
            // Act
            
            var result = await _productService.GetListAsync();

            // Assert
            
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAsync_ExistingProduct_ReturnsProduct()
        {
            // Arrange
            var product = _fixture.Create<ProductDto>();

            _productApiClientMock.Setup(x => x.GetProductAsync(product.Id)).ReturnsAsync(product);

            // Act
            
            var result = await _productService.GetAsync(product.Id);

            // Assert
            
            result.Should().BeEquivalentTo(product);
        }

        [Fact]
        public async Task GetAsync_NonExistingProduct_ReturnsNull()
        {
            // Arrange
            
            _productApiClientMock.Setup(x => x.GetProductAsync(It.IsAny<Guid>()))!.ReturnsAsync((ProductDto?)null);
            
            // Act
            
            var result = await _productService.GetAsync(Guid.NewGuid());

            // Assert
            
            result.Should().BeNull();
        }

        [Fact]
        public async Task AddAsync_AddsProduct()
        {
            // Arrange
            
            var product = _fixture.Create<ProductDto>();
            
            _productApiClientMock.Setup(x => x.CreateProductAsync(product))!.ReturnsAsync(product);
            
            // Act
            
            var result = await _productService.AddAsync(product);
            
            // Assert

            result.Should().BeEquivalentTo(product);
        }

        [Fact]
        public async Task UpdateAsync_ExistingProduct_UpdatesProduct()
        {
            // Arrange
            
            var product = _fixture.Create<ProductDto>();
            
            _productApiClientMock.Setup(x => x.UpdateProductAsync(product.Id, product))!.ReturnsAsync(product);
            
            // Act
            
            var result = await _productService.UpdateAsync(product);
            
            // Assert

            result.Should().BeEquivalentTo(product);
        }

        [Fact]
        public async Task Update_NonExistingProduct_ReturnsNull()
        {
            // Arrange
            
            _productApiClientMock.Setup(x => x.UpdateProductAsync(It.IsAny<Guid>(),It.IsAny<ProductDto>()))!.ReturnsAsync((ProductDto?) null);

            // Act
            
            var result = await _productService.UpdateAsync(_fixture.Create<ProductDto>());
            
            // Assert
            
            result.Should().BeNull();
        }

        [Fact]
        public async Task DeleteAsync_ExistingProduct_ApiClientDeleteProductAsyncCalled()
        {
            // Arrange
            
            var product = _fixture.Create<Product>();
            
            // Act
            
            await _productService.DeleteAsync(product.Id);

            // Assert
            
            _productApiClientMock.Verify(x => x.DeleteProductAsync(product.Id), Times.Once);
        }
    }