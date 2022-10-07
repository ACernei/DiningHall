using System.Text;
using System.Text.Json;
using DiningHall.Models;

namespace DiningHall.Services;

public class OrderService : IOrderService
{
    private readonly HttpClient httpClient;
    private readonly ILogger<OrderService> logger;
    private readonly Random random = new();
    
    public OrderService(HttpClient httpClient, ILogger<OrderService> logger)
    {
        this.httpClient = httpClient;
        this.logger = logger;
    }

    public async Task PostAsync()
    {
        var order = new Order
        {
            Id = random.Next(),
            Items = new List<int> { 3, 4, 4, 2 },
            Priority = 3,
            MaxWait = 45
        };
        using var response = await httpClient.PostAsJsonAsync("order", order);
        Console.WriteLine(response.ToString());
    }
}
