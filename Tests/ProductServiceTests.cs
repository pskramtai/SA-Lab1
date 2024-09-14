using AutoFixture;
using FluentAssertions;
using Lab1.Models;
using Lab1.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Xunit;

namespace Tests;

public class ProductServiceTests
    {
        private readonly Fixture _fixture = new();

        private readonly ProductService _productService = new();

        [Fact]
        public void GetList_EmptyList_ReturnsEmptyCollection()
        {
            // Act
            
            var result = _productService.GetList();

            // Assert
            
            result.Should().BeEmpty();
        }

        [Fact]
        public void Get_ExistingProduct_ReturnsProduct()
        {
            // Arrange
            var product = _fixture.Create<Product>();
            _productService.Add(product);

            // Act
            
            var result = _productService.Get(product.Id);

            // Assert
            
            result.Should().BeEquivalentTo(product);
        }

        [Fact]
        public void Get_NonExistingProduct_ReturnsNull()
        {
            // Act
            
            var result = _productService.Get(Guid.NewGuid());

            // Assert
            
            result.Should().BeNull();
        }

        [Fact]
        public void Add_ValidProduct_IncreasesProductListSize()
        {
            // Arrange
            
            var product = _fixture.Create<Product>();
            
            // Act
            
            _productService.Add(product);
            
            var result = _productService.GetList();

            // Assert
            
            result.Should().Contain(product);
        }

        [Fact]
        public void Update_ExistingProduct_UpdatesProduct()
        {
            // Arrange
            
            var productId = Guid.NewGuid();
            var initialProduct = _fixture
                .Build<Product>()
                .With(x => x.Id, productId)
                .Create();
            
            _productService.Add(initialProduct);

            var updatedProduct = _fixture
                .Build<Product>()
                .With(x => x.Id, productId)
                .Create();

            // Act
            
            _productService.Update(updatedProduct);
            
            var result = _productService.Get(productId);

            // Assert
            
            result.Should().BeEquivalentTo(updatedProduct);
        }

        [Fact]
        public void Update_NonExistingProduct_DoesNotChangeList()
        {
            // Arrange
            
            var initialProduct = _fixture.Create<Product>();
            
            _productService.Add(initialProduct);

            var nonExistingProduct = _fixture.Create<Product>();

            // Act
            
            _productService.Update(nonExistingProduct);
            
            var result = _productService.GetList();

            // Assert
            result.Should().ContainSingle();
            result.Should().Contain(initialProduct);
        }

        [Fact]
        public void Remove_ExistingProduct_RemovesProduct()
        {
            // Arrange
            var product = _fixture.Create<Product>();
            _productService.Add(product);

            // Act
            _productService.Remove(product.Id);
            var result = _productService.GetList();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void Remove_NonExistingProduct_DoesNothing()
        {
            // Arrange
            
            var nonExistingProductId = Guid.NewGuid();

            // Act
            
            _productService.Remove(nonExistingProductId);
            
            var result = _productService.GetList();

            // Assert
            
            result.Should().BeEmpty();
        }
    }