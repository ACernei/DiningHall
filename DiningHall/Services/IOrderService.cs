using DiningHall.Models;

namespace DiningHall.Services;

public interface IOrderService
{
    public Order GenerateOrder();
    public Task PostAsync(Order order);
}
