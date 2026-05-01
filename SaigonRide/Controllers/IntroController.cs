using Microsoft.AspNetCore.Mvc;

public class IntroController : Controller
{
    public IActionResult Index() => View();
}