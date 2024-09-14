using System.ComponentModel.DataAnnotations;

namespace Lab1.Models;

public record Product
(
    Guid Id,
    
    [Required]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "The product name must be between 3 and 100 characters.")]
    string Name,
    
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Price must be a positive number.")]
    int Price,
    
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a positive number.")]
    int Quantity,
    
    [Required]
    [EnumDataType(typeof(ProductCategory), ErrorMessage = "Invalid product category.")]
    ProductCategory ProductCategory
);

public enum ProductCategory
{
    Electronics,
    Clothing,
    Food,
    Other
}