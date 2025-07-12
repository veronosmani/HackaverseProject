using Microsoft.AspNetCore.Mvc;
using HackaverseProject.Models;
using System.Linq;

namespace HackaverseProject.Controllers
{
    public class IngredientController : Controller
    {
        private readonly AppDbContext _context;
        public IngredientController(AppDbContext context) => _context = context;

        public IActionResult Index() => View(_context.Ingredients.ToList());
    }
}