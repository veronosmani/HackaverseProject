// Controllers/HomeController.cs
using Microsoft.AspNetCore.Mvc;
using HackaverseProject.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace HackaverseProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context) => _context = context;

        [HttpGet]
        public IActionResult Index(string mealType = "", int? prepTime = null)
        {
            var pantry = _context.PantryItems.Include(p => p.Ingredient).ToList();

            var mealsQuery = _context.Meals
                .Include(m => m.MealIngredients)
                    .ThenInclude(mi => mi.Ingredient)
                .AsQueryable();

            if (!string.IsNullOrEmpty(mealType))
                mealsQuery = mealsQuery.Where(m => m.Category == mealType);

            if (prepTime.HasValue)
                mealsQuery = mealsQuery.Where(m => m.PrepTime <= prepTime.Value);

            var allMeals = _context.Meals
                .Include(m => m.MealIngredients)
                    .ThenInclude(mi => mi.Ingredient)
                .ToList();
            var cookableMeals = new List<Meal>();

            foreach (var meal in allMeals)
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

            var vm = new MealsViewModel
            {
                CookableMeals = cookableMeals,
                AllMeals = allMeals,
                PantryList = pantry
            };

            return View(vm);
        }


        [HttpPost]
        public IActionResult AddToFridge(string ingredient, double quantity, string unit, string type)
        {
            var ingr = _context.Ingredients.FirstOrDefault(i => i.Name.ToLower() == ingredient.ToLower());

            if (ingr == null)
            {
                ingr = new Ingredient { Name = ingredient, Unit = unit, Type = type };
                _context.Ingredients.Add(ingr);
                _context.SaveChanges();
            }

            var pantryItem = _context.PantryItems.FirstOrDefault(p => p.IngredientId == ingr.IngredientId);

            if (pantryItem != null)
                pantryItem.QuantityAvailable += quantity;
            else
                _context.PantryItems.Add(new PantryItem
                {
                    IngredientId = ingr.IngredientId,
                    QuantityAvailable = quantity
                });

            _context.SaveChanges();
            TempData["FridgeItem"] = $"{quantity} {unit} {ingredient}";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult AddMeal(string name, string category, int prepTime, string ingredientIds, string quantities)
        {
            var meal = new Meal { Name = name, Category = category, PrepTime = prepTime, Instructions = "" };
            _context.Meals.Add(meal);
            _context.SaveChanges();

            var ingredientIdList = ingredientIds.Split(',').Select(id => int.Parse(id.Trim())).ToList();
            var quantityList = quantities.Split(',').Select(q => double.Parse(q.Trim())).ToList();

            for (int i = 0; i < ingredientIdList.Count; i++)
            {
                _context.MealIngredients.Add(new MealIngredient
                {
                    MealId = meal.MealId,
                    IngredientId = ingredientIdList[i],
                    QuantityPerServing = quantityList[i]
                });
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeletePantryItem(int id)
        {
            var item = _context.PantryItems.Find(id);
            if (item != null)
            {
                _context.PantryItems.Remove(item);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public IActionResult TestData()
        {
            var meals = _context.Meals
                .Include(m => m.MealIngredients)
                .ThenInclude(mi => mi.Ingredient)
                .ToList();

            return Json(meals.Select(m => new {
                m.MealId,
                m.Name,
                Ingredients = m.MealIngredients.Select(mi => new {
                    mi.IngredientId,
                    mi.Ingredient.Name,
                    mi.QuantityPerServing
                })
            }));
        }

        [HttpPost]
        public IActionResult DeleteMeal(int id)
        {
            var meal = _context.Meals.Include(m => m.MealIngredients).FirstOrDefault(m => m.MealId == id);
            if (meal != null)
            {
                _context.MealIngredients.RemoveRange(meal.MealIngredients);
                _context.Meals.Remove(meal);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
