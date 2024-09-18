using System.ComponentModel.DataAnnotations;

namespace Lab1.Models;

public record Product(
    Guid Id,
    string Name,
    string Description,
    int Price,
    int Quantity,
    ProductCategory ProductCategory
) : IValidatableObject
{
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var results = new List<ValidationResult>();

        if (string.IsNullOrWhiteSpace(Name) || Name.Length < 3 || Name.Length > 10)
        {
            results.Add(new ValidationResult("The product name must be between 3 and 10 characters.", [nameof(Name)]));
        }

        if (string.IsNullOrWhiteSpace(Description) || Description.Length < 3 || Description.Length > 100)
        {
            results.Add(new ValidationResult("The product description must be between 3 and 100 characters.", [nameof(Description)]));
        }
        
        if (Price < 0)
        {
            results.Add(new ValidationResult("Price must be a non negative number", [nameof(Price)]));
        }
        
        if (Quantity < 0)
        {
            results.Add(new ValidationResult("Quantity must be a non negative number", [nameof(Quantity)]));
        }
        
        if (!Enum.IsDefined(typeof(ProductCategory), ProductCategory))
        {
            results.Add(new ValidationResult("Invalid product category.", [nameof(ProductCategory)]));
        }
    
        return results;
    }
}

public enum ProductCategory
{
    Electronics,
    Clothing,
    Food,
    Other
}