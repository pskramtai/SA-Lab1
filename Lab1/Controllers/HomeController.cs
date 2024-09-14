using Microsoft.AspNetCore.Mvc;

namespace Lab1.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}