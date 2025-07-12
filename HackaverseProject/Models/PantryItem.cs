public class PantryItem
{
    public int PantryItemId { get; set; }
    public int IngredientId { get; set; }
    public double QuantityAvailable { get; set; }

    // Optional User reference if multi-user support:
    public int? UserId { get; set; }  // <- Does this exist?
    public User User { get; set; }

    public Ingredient Ingredient { get; set; }
}
