using DiningHall.Models;
using DiningHall.Services;
using Microsoft.AspNetCore.Mvc;

namespace DiningHall.Controllers;

[ApiController]
[Route("distribution")]
public class DistributionController : ControllerBase
{
    private readonly TableManager tableManager;
    private readonly ILogger<DistributionController> logger;

    public DistributionController(TableManager tableManager,ILogger<DistributionController> logger)
    {
        this.tableManager = tableManager;
        this.logger = logger;
    }

    [HttpPost]
    public IActionResult Post(Order order)
    {
        this.tableManager.ServeOrder(order.Id);
        return Ok();
    }
}
