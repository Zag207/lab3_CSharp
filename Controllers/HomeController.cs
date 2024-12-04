using Microsoft.AspNetCore.Mvc;

namespace Lab3_CSharp;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}