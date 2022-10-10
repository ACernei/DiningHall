using System.Text;
using System.Text.Json;
using DiningHall.Models;

namespace DiningHall.Services;

public class OrderService : IOrderService
{
    private readonly HttpClient httpClient;
    private readonly ILogger<OrderService> logger;
    private readonly Random random = new();
    private static int orderId = 0;
    private object orderIdlock = new object();
    
    public OrderService(HttpClient httpClient, ILogger<OrderService> logger)
    {
        this.httpClient = httpClient;
        this.logger = logger;
    }

    public Order GenerateOrder()
    {
        int newOrderId;
        lock (orderIdlock) { newOrderId = ++orderId; }
        return new Order
        {
            Id = newOrderId,
            Items = new List<int>(),
            CreationTime = DateTimeOffset.UtcNow
        };
    }

    public async Task PostAsync(Order order)
    {
        using var response = await httpClient.PostAsJsonAsync("order", order);
        this.logger.LogInformation(response.ToString());
    }
}
