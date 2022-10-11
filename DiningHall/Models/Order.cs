using System.Text.Json.Serialization;

namespace DiningHall.Models;

public class Order
{
    public int Id { get; set; }
    public List<int> Items { get; init; }
    public int MaxWait { get; init; }
    public int Priority { get; init; }

    [JsonIgnore]
    public DateTimeOffset CreationTime { get; init; }
    [JsonIgnore]
    public DateTimeOffset CompletionTime { get; set; }
    [JsonIgnore]
    public int TableId { get; set; }
}
