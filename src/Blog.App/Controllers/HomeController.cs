using Microsoft.AspNetCore.Mvc;

namespace Blog.App.Controllers
{
    public class HomeController : Controller
    {
        
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Post");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        
    }
}
