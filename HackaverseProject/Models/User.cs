public class User
{
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }

    public ICollection<PantryItem> PantryItems { get; set; }
    public ICollection<CookHistory> CookHistories { get; set; }
}
