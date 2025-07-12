using Microsoft.AspNetCore.Mvc;
using HackaverseProject.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

namespace HackaverseProject.Controllers
{
    public class SuggestionController : Controller
    {
        private readonly AppDbContext _context;
        public SuggestionController(AppDbContext context) => _context = context;

        public IActionResult Index()
        {
            var pantry = _context.PantryItems.ToList();
            var meals = _context.Meals
                .Include(m => m.MealIngredients)
                .ThenInclude(mi => mi.Ingredient)
                .ToList();

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
                if (canCook) cookableMeals.Add(meal);
            }

            return View(cookableMeals);
        }
    }
}
