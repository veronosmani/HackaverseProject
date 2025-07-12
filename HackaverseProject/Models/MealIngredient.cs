public class MealIngredient
{
    public int MealId { get; set; }
    public Meal Meal { get; set; }

    public int IngredientId { get; set; }
    public Ingredient Ingredient { get; set; }

    public double QuantityPerServing { get; set; }
}
