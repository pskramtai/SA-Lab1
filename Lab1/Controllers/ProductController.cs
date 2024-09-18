using Lab1.Extensions;
using Lab1.Models;
using Lab1.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Lab1.Controllers;

[Route("Products")]
public class ProductController(IProductService productService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var products = (await productService
            .GetListAsync())
            .Select(x => x.ToModel())
            .ToList();
        
        return View(products);
    }
    
    [HttpGet("Edit/{id:guid}")]
    public async Task<IActionResult> Edit(Guid id)
    {
        var product = await productService.GetAsync(id);

        return product is not null ? View(product.ToModel()) : NotFound();
    }
    
    [HttpGet("Add")]
    public IActionResult Add()
    {
        return View();
    }
    
    [HttpPost("Add")]
    public async Task<IActionResult> Add(Product product)
    {
        if (!ModelState.IsValid)
        {
            return View(product);
        }
        
        var newProduct = await productService.AddAsync(product.ToProductDto()); 
            
        return RedirectToAction("Edit", new { id = newProduct.Id });
    }
    
    [HttpPost("Update")]
    public async Task<IActionResult> Update(Product product)
    {
        if (!ModelState.IsValid)
        {
            return View("Edit", product);
        }
        
        var updatedProduct = await productService.UpdateAsync(product.ToProductDto());
        
        if (updatedProduct is null)
        {
            TempData["ErrorMessage"] = "Product update failed.";
            return RedirectToAction("Error", "Home");
        }
        
        return RedirectToAction("Edit", new { id = updatedProduct.Id });
    }
    
    [HttpPost("Remove")]
    public async Task<IActionResult> Remove(Guid guid)
    {
        await productService.DeleteAsync(guid);
        
        return RedirectToAction("Index");
    }
}