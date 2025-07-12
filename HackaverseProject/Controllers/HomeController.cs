// Controllers/HomeController.cs
using Microsoft.AspNetCore.Mvc;
using HackaverseProject.Models;
using Microsoft.EntityFrameworkCore;

namespace HackaverseProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context) => _context = context;

        [HttpGet]
        public IActionResult Index(string mealType = "", int? prepTime = null)
        {
            var pantry = _context.PantryItems.ToList();

            var mealsQuery = _context.Meals
                .Include(m => m.MealIngredients)
                .ThenInclude(mi => mi.Ingredient)
                .AsQueryable();

            if (!string.IsNullOrEmpty(mealType))
                mealsQuery = mealsQuery.Where(m => m.Category == mealType);

            if (prepTime.HasValue)
                mealsQuery = mealsQuery.Where(m => m.PrepTime <= prepTime.Value);

            var meals = mealsQuery.ToList();
            var cookableMeals = new List<Meal>();

            foreach (var meal in meals)
            {
                bool canCook = true;
                foreach (var mi in meal.MealIngredients)
                {
                    var pantryItem = pantry.FirstOrDefault(p => p.IngredientId == mi.IngredientId);
                    if (pantryItem == null || pantryItem.QuantityAvailable < mi.QuantityPerServing)
                    {
                        canCook = false;
                        break;
                    }
                }
                if (canCook)
                    cookableMeals.Add(meal);
            }

            return View(cookableMeals);
        }

        [HttpPost]
        public IActionResult AddToFridge(string ingredient)
        {
            if (!string.IsNullOrWhiteSpace(ingredient))
            {
                var parts = ingredient.Split(' ', 2);
                if (parts.Length == 2 && double.TryParse(parts[0], out double quantity))
                {
                    var name = parts[1].Trim().ToLower();
                    var ingr = _context.Ingredients.FirstOrDefault(i => i.Name.ToLower() == name);

                    if (ingr != null)
                    {
                        var pantryItem = _context.PantryItems.FirstOrDefault(p => p.IngredientId == ingr.IngredientId);
                        if (pantryItem != null)
                            pantryItem.QuantityAvailable += quantity;
                        else
                            _context.PantryItems.Add(new PantryItem { IngredientId = ingr.IngredientId, QuantityAvailable = quantity });

                        _context.SaveChanges();
                        TempData["FridgeItem"] = $"{quantity} {name}";
                    }
                    else
                    {
                        TempData["FridgeItem"] = $"Ingredient '{name}' not found.";
                    }
                }
                else
                {
                    TempData["FridgeItem"] = "Please enter a valid format, e.g. '3 eggs'";
                }
            }

            return RedirectToAction("Index");
        }
    }
}
