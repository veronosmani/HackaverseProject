using Microsoft.AspNetCore.Mvc;
using HackaverseProject.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace HackaverseProject.Controllers
{
    public class MealController : Controller
    {
        private readonly AppDbContext _context;
        public MealController(AppDbContext context) => _context = context;

        public IActionResult Index() => View(
            _context.Meals.Include(m => m.MealIngredients).ThenInclude(mi => mi.Ingredient).ToList()
        );

        public IActionResult Details(int id)
        {
            var meal = _context.Meals
                .Include(m => m.MealIngredients)
                .ThenInclude(mi => mi.Ingredient)
                .FirstOrDefault(m => m.MealId == id);
            return View(meal);
        }
    }
}