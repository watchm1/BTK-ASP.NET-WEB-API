namespace Entities.Models;

public class Book
{
    public int Id { get; set; }
    public String Title { get; set; } = null!;
    public decimal Price { get; set; }
}