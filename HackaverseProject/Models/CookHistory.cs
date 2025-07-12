public class CookHistory
{
    public int CookHistoryId { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public int MealId { get; set; }
    public Meal Meal { get; set; }

    public DateTime DateCooked { get; set; }
    public int ServingsMade { get; set; }
    public int ServingsLeft { get; set; }
}
