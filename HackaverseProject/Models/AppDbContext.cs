using Microsoft.EntityFrameworkCore;
using HackaverseProject.Models;

namespace HackaverseProject.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<MealIngredient> MealIngredients { get; set; }
        public DbSet<PantryItem> PantryItems { get; set; }
        public DbSet<CookHistory> CookHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MealIngredient>()
                .HasKey(mi => new { mi.MealId, mi.IngredientId });

            modelBuilder.Entity<MealIngredient>()
                .HasOne(mi => mi.Meal)
                .WithMany(m => m.MealIngredients)
                .HasForeignKey(mi => mi.MealId);

            modelBuilder.Entity<MealIngredient>()
                .HasOne(mi => mi.Ingredient)
                .WithMany(i => i.MealIngredients)
                .HasForeignKey(mi => mi.IngredientId);
        }
    }
}
