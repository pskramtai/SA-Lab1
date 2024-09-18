using AutoFixture;
using FluentAssertions;
using FluentValidation.TestHelper;
using WebApi.Presentation.Models.Request;
using WebApi.Presentation.Validators;
using WebApi.Shared;

namespace WebApi.Tests;

public class ProductRequestValidatorTests
{
    private readonly ProductRequestValidator _validator = new();
    private readonly Fixture _fixture = new();

    [Fact]
    public void Validate_WhenNameIsTooShort_ShouldReturnValidationError()
    {
        // Arrange
        var model = _fixture.Build<ProductRequest>()
            .With(x => x.Name, "AB")
            .Create();

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("The product name must be between 3 and 10 characters.");
    }

    [Fact]
    public void Validate_WhenNameIsTooLong_ShouldReturnValidationError()
    {
        // Arrange
        var model = _fixture.Build<ProductRequest>()
            .With(x => x.Name, new string('A', 11))
            .Create();

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("The product name must be between 3 and 10 characters.");
    }

    [Fact]
    public void Validate_WhenDescriptionIsTooShort_ShouldReturnValidationError()
    {
        // Arrange
        var model = _fixture.Build<ProductRequest>()
            .With(x => x.Description, "AB")
            .Create();

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description)
            .WithErrorMessage("The product description must be between 3 and 100 characters.");
    }
    
    [Fact]
    public void Validate_WhenDescriptionIsTooLong_ShouldReturnValidationError()
    {
        // Arrange
        var model = _fixture.Build<ProductRequest>()
            .With(x => x.Description, new string('A', 101))
            .Create();

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description)
            .WithErrorMessage("The product description must be between 3 and 100 characters.");
    }

    [Fact]
    public void Validate_WhenPriceIsNegative_ShouldReturnValidationError()
    {
        // Arrange
        var model = _fixture.Build<ProductRequest>()
            .With(x => x.Price, -1)
            .Create();

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Price)
            .WithErrorMessage("Price must be a non negative number.");
    }

    [Fact]
    public void Validate_WhenQuantityIsNegative_ShouldReturnValidationError()
    {
        // Arrange
        var model = _fixture.Build<ProductRequest>()
            .With(x => x.Quantity, -1)
            .Create();

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Quantity)
            .WithErrorMessage("Quantity must be a non negative number.");
    }

    [Fact]
    public void Validate_WhenProductCategoryIsInvalid_ShouldReturnValidationError()
    {
        // Arrange
        var model = _fixture.Build<ProductRequest>()
            .With(x => x.ProductCategory, (ProductCategory)999)
            .Create();

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ProductCategory)
            .WithErrorMessage("Invalid product category.");
    }

    [Fact]
    public void Validate_WhenRequestIsValid_ShouldNotReturnValidationError()
    {
        // Arrange
        var model = _fixture.Build<ProductRequest>()
            .With(x => x.Name, "ValidName")
            .With(x => x.Description, "Valid description within range.")
            .With(x => x.Price, 10)
            .With(x => x.Quantity, 5)
            .With(x => x.ProductCategory, ProductCategory.Electronics)
            .Create();

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}