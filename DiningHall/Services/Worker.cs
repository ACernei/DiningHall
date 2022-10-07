namespace DiningHall.Services;

public class Worker : BackgroundService
{
    private readonly IOrderService orderService;
    private readonly ILogger<Worker> logger;

    public Worker(IOrderService orderService, ILogger<Worker> logger)
    {
        this.orderService = orderService;
        this.logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            this.logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await orderService.PostAsync();
            await Task.Delay(2000, stoppingToken);
        }
    }
}
