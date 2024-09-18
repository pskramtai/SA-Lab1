using System.ComponentModel.DataAnnotations;
using AutoFixture;
using FluentAssertions;
using Lab1.Models;
using Xunit;

namespace Tests.Models;

public class ProductTests
{
    private readonly Fixture _fixture;

    public ProductTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public void Validate_WhenNameIsTooShort_ShouldReturnValidationError()
    {
        // Arrange
        var product = _fixture.Build<Product>()
            .With(p => p.Name, "ab")
            .Create();

        // Act
        var result = ValidateModel(product);

        // Assert
        result.Should().ContainSingle(v => v.ErrorMessage == "The product name must be between 3 and 10 characters.");
    }

    [Fact]
    public void Validate_WhenNameIsTooLong_ShouldReturnValidationError()
    {
        // Arrange
        var product = _fixture.Build<Product>()
            .With(p => p.Name, new string('A', 11))
            .Create();

        // Act
        var result = ValidateModel(product);

        // Assert
        result.Should().ContainSingle(v => v.ErrorMessage == "The product name must be between 3 and 10 characters.");
    }

    [Fact]
    public void Validate_WhenDescriptionIsTooShort_ShouldReturnValidationError()
    {
        // Arrange
        var product = _fixture.Build<Product>()
            .With(p => p.Description, "ab")
            .Create();

        // Act
        var result = ValidateModel(product);

        // Assert
        result.Should().ContainSingle(v => v.ErrorMessage == "The product description must be between 3 and 100 characters.");
    }

    [Fact]
    public void Validate_WhenDescriptionIsTooLong_ShouldReturnValidationError()
    {
        // Arrange
        var product = _fixture.Build<Product>()
            .With(p => p.Description, new string('a', 101))
            .Create();

        // Act
        var result = ValidateModel(product);

        // Assert
        result.Should().ContainSingle(v => v.ErrorMessage == "The product description must be between 3 and 100 characters.");
    }

    [Fact]
    public void Validate_WhenPriceIsNegative_ShouldReturnValidationError()
    {
        // Arrange
        var product = _fixture.Build<Product>()
            .With(p => p.Price, -1)
            .Create();

        // Act
        var result = ValidateModel(product);

        // Assert
        result.Should().ContainSingle(v => v.ErrorMessage == "Price must be a non negative number");
    }

    [Fact]
    public void Validate_WhenQuantityIsNegative_ShouldReturnValidationError()
    {
        // Arrange
        var product = _fixture.Build<Product>()
            .With(p => p.Quantity, -1)
            .Create();

        // Act
        var result = ValidateModel(product);

        // Assert
        result.Should().ContainSingle(v => v.ErrorMessage == "Quantity must be a non negative number");
    }

    [Fact]
    public void Validate_WhenProductCategoryIsInvalid_ShouldReturnValidationError()
    {
        // Arrange
        var product = _fixture.Build<Product>()
            .With(p => p.ProductCategory, (ProductCategory)999)
            .Create();

        // Act
        var result = ValidateModel(product);

        // Assert
        result.Should().ContainSingle(v => v.ErrorMessage == "Invalid product category.");
    }

    [Fact]
    public void Validate_WhenProductIsValid_ShouldNotReturnAnyValidationErrors()
    {
        // Arrange
        var product = _fixture.Build<Product>()
            .With(p => p.Name, "ValidName")
            .With(p => p.Description, "Valid Description")
            .With(p => p.Price, 10)
            .With(p => p.Quantity, 5)
            .With(p => p.ProductCategory, ProductCategory.Electronics)
            .Create();

        // Act
        var result = ValidateModel(product);

        // Assert
        result.Should().BeEmpty();
    }

    private List<ValidationResult> ValidateModel(Product product)
    {
        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(product, null, null);
        Validator.TryValidateObject(product, context, validationResults, true);

        return validationResults;
    }
}