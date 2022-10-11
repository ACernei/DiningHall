using System.Collections.Concurrent;
using DiningHall.Models;
using Microsoft.Extensions.Options;

namespace DiningHall.Services;

public class TableManager : BackgroundService
{
    private readonly IOrderService orderService;
    private readonly DiningHallOptions config;
    private readonly ILogger<TableManager> logger;
    
    private readonly Random random = new();
    private readonly Semaphore waitersSemaphore;
    private ConcurrentDictionary<int, Order> tableOrders = new();

    public TableManager(
        IOrderService orderService,
        IOptions<DiningHallOptions> options,
        ILogger<TableManager> logger)
    {
        this.orderService = orderService;
        this.config = options.Value;
        this.logger = logger;
        
        this.waitersSemaphore = new Semaphore(0, this.config.Waiters);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        for (int i = 0; i < config.Tables; i++)
        {
            var tableId = i;
            Task.Run(() => ServeTable(tableId), stoppingToken);
        }

        this.waitersSemaphore.Release(config.Waiters);
        return Task.CompletedTask;
    }

    private async Task ServeTable(int tableId)
    {
        // wait for client to arrive
        await Task.Delay(this.random.Next(1, 5) * this.config.TimeUnit);

        // client arrived ant thinks of an order
        var order = orderService.GenerateOrder();
        order.TableId = tableId;

        // wait for waiter
        this.waitersSemaphore.WaitOne();

        // waiter takes order
        await Task.Delay(this.random.Next(2, 3) * this.config.TimeUnit);

        // send order to kitchen
        logger.LogInformation($"TABLE {tableId} SENDS ORDER {order.Id}");
        await orderService.PostAsync(order);
        this.tableOrders[order.Id] = order;
        
        // waiter finished processing order
        this.waitersSemaphore.Release();
    }

    public void ServeOrder(int orderId)
    {
        if (!this.tableOrders.TryGetValue(orderId, out var order))
        {
            this.logger.LogWarning($"There is no table for order {orderId}.");
            return;
        }
        this.logger.LogInformation($"CLIENT AT TABLE {order.TableId} EATS ORDER {order.Id}");
        order.CompletionTime = DateTimeOffset.UtcNow;

        // wait for client to eat and then table is free again
        Task.Run(() => Task.Delay(this.random.Next(1, 5) * this.config.TimeUnit)
            .ContinueWith(_ => ServeTable(order.TableId)));
    }
}
