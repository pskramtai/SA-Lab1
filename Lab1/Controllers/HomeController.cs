using Lab1.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lab1.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        var errorMessage = TempData["ErrorMessage"] as string;
        return View(new ErrorViewModel { ErrorMessage = errorMessage });
    }
}