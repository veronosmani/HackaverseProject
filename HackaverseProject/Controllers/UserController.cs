using Microsoft.AspNetCore.Mvc;
using HackaverseProject.Models;
using System.Linq;

namespace HackaverseProject.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _context;
        public UserController(AppDbContext context) => _context = context;

        public IActionResult Index() => View(_context.Users.ToList());
    }
}