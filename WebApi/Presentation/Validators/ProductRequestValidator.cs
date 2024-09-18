using FluentValidation;
using WebApi.Presentation.Models.Request;
using WebApi.Shared;

namespace WebApi.Presentation.Validators;

public class ProductRequestValidator : AbstractValidator<ProductRequest>
{
    public ProductRequestValidator()
    {
        RuleFor(x => x.Name)
            .Must(x => x.Length >= 3 && x.Length <= 10)
            .WithMessage("The product name must be between 3 and 10 characters.");
        
        RuleFor(x => x.Description)
            .Must(x => x.Length >= 3 && x.Length <= 100)
            .WithMessage("The product name must be between 3 and 100 characters.");

        RuleFor(x => x.Price)
            .Must(x => x >= 0)
            .WithMessage("Price must be a non negative number.");

        RuleFor(x => x.Quantity)
            .Must(x => x >= 0)
            .WithMessage("Quantity must be a non negative number.");

        RuleFor(x => x.ProductCategory)
            .Must(x => Enum.IsDefined(typeof(ProductCategory), x))
            .WithMessage("Invalid product category.");
    }
}