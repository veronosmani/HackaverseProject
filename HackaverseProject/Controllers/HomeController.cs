using Microsoft.AspNetCore.Mvc;

namespace HackaverseProject.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddToFridge(string ingredient)
        {
            TempData["FridgeItem"] = ingredient;
            return RedirectToAction("Index");
        }
    }
}
