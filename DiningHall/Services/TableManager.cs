using DiningHall.Models;

namespace DiningHall.Services;

public class TableManager : BackgroundService
{
    private readonly IOrderService orderService;
    private readonly IConfiguration configuration;
    private readonly ILogger<TableManager> logger;
    private readonly int timeUnit;
    private readonly Random random = new();
    private Semaphore waitersSemaphore;

    public TableManager(
        IOrderService orderService,
        IConfiguration configuration,
        ILogger<TableManager> logger)
    {
        this.orderService = orderService;
        this.configuration = configuration;
        this.logger = logger;
        this.timeUnit = configuration.GetValue<int>("TimeUnit");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var tables = configuration.GetValue<int>("Tables");
        var waiters = configuration.GetValue<int>("Waiters");

        this.waitersSemaphore = new Semaphore(0, waiters);

        for (int i = 0; i < tables; i++)
        {
            var i1 = i;
            Task.Run(() => ServeTable(i1), stoppingToken);
        }

        this.waitersSemaphore.Release(waiters);

        // while (!stoppingToken.IsCancellationRequested)
        // {
        //     this.logger.LogInformation("TableManager running at: {time}", DateTimeOffset.Now);
        //     await orderService.PostAsync();
        //     await Task.Delay(2000, stoppingToken);
        // }
    }

    private async void ServeTable(int tableId)
    {
        // wait for client to arrive
        await Task.Delay(this.random.Next(1, 5) * this.timeUnit);

        // client arrived ant thinks of an order
        var order = orderService.GenerateOrder(); // with timestamp??

        // wait for waiter
        this.waitersSemaphore.WaitOne();

        // waiter takes order
        await Task.Delay(this.random.Next(2, 3) * this.timeUnit);

        // waiter took order
        this.waitersSemaphore.Release();

        // send order to kitchen
        logger.LogInformation($"TABLE {tableId} SENDS ORDER {order.Id}");
        await orderService.PostAsync(order);
    }
}
