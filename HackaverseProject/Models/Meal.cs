public class Meal
{
    public int MealId { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }  // e.g., Breakfast, Lunch, Dinner
    public string Instructions { get; set; }
    public int PrepTime { get; set; } // in minutes

    public ICollection<MealIngredient> MealIngredients { get; set; }
    public ICollection<CookHistory> CookHistories { get; set; }
}
