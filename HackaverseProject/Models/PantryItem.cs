public class PantryItem
{
    public int PantryItemId { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public int IngredientId { get; set; }
    public Ingredient Ingredient { get; set; }

    public double QuantityAvailable { get; set; }
}
