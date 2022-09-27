using Microsoft.AspNetCore.Mvc;

namespace DiningHall.Controllers;

[ApiController]
[Route("[controller]")]
public class DistributionController : ControllerBase
{
    private readonly ILogger<DistributionController> logger;

    public DistributionController(ILogger<DistributionController> logger)
    {
        this.logger = logger;
    }

    [HttpPost]
    public IEnumerable<object> Post()
    {
        return new List<object> {string.Empty};
    }
}
