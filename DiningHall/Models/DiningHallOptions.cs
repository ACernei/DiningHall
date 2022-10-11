namespace DiningHall.Models;

public class DiningHallOptions
{
    public const string DiningHall = "DiningHall";
    public int Tables { get; set; }
    public int Waiters { get; set; }
    public int TimeUnit { get; set; }
    public List<MenuItem> Menu { get; set; }
}
