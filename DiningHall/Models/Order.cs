namespace DiningHall.Models;

public class Order
{
    private readonly Guid guid;
    public int Id { get; set; }
    public List<int> Items { get; init; }
    public int MaxWait { get; init; }
    public int Priority { get; init; }


    public DateTimeOffset CreationTime { get; init; }
}
