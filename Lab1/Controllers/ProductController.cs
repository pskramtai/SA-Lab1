using Lab1.Models;
using Lab1.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Lab1.Controllers;

[Route("Products")]
public class ProductController(IProductService productService) : Controller
{
    public IActionResult Index()
    {
        var products = productService.GetList();
        
        return View(products);
    }
    
    [HttpGet("Edit/{id:guid}")]
    public IActionResult Edit(Guid id)
    {
        var product = productService.Get(id);

        return product is not null ? View(product) : NotFound();
    }
    
    [HttpGet("Add")]
    public IActionResult Add()
    {
        return View();
    }
    
    [HttpPost("Add")]
    public IActionResult Add(Product product)
    {
        if (!ModelState.IsValid)
        {
            return View(product);
        }
        
        var newProduct = product with { Id = Guid.NewGuid() };
        productService.Add(newProduct); 
            
        return RedirectToAction("Edit", new { id = newProduct.Id });
    }
    
    [HttpPost("Update")]
    public IActionResult Update(Product product)
    {
        if (!ModelState.IsValid)
        {
            return View("Edit", product);
        }
        
        productService.Update(product);
        
        return RedirectToAction("Edit", new { id = product.Id });
    }
    
    [HttpPost("Remove")]
    public IActionResult Remove(Guid guid)
    {
        productService.Remove(guid);
        
        return RedirectToAction("Index");
    }
}