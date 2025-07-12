public class Ingredient
{
    public int IngredientId { get; set; }

    public string Name { get; set; }

    public string Unit { get; set; }

    // New field: solid or liquid
    public string Type { get; set; }

    public ICollection<MealIngredient> MealIngredients { get; set; }

    public ICollection<PantryItem> PantryItems { get; set; }
}
