using Microsoft.AspNetCore.Mvc;
using HackaverseProject.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HackaverseProject.Controllers
{
    public class PantryController : Controller
    {
        private readonly AppDbContext _context;
        public PantryController(AppDbContext context) => _context = context;

        public IActionResult Index() => View(
            _context.PantryItems.Include(p => p.Ingredient).ToList()
        );

        [HttpPost]
        public IActionResult Add(int ingredientId, double quantity)
        {
            var item = _context.PantryItems.FirstOrDefault(p => p.IngredientId == ingredientId);
            if (item != null)
                item.QuantityAvailable += quantity;
            else
                _context.PantryItems.Add(new PantryItem { IngredientId = ingredientId, QuantityAvailable = quantity });

            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
