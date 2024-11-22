using dot_net_api.Tmp;
using Microsoft.AspNetCore.Mvc;

namespace dot_net_api.Controllers;

[ApiController]
[Route("delegate-test")]
public class SomeDelegateTestsController: ControllerBase
{
    private readonly ILogger<SomeDelegateTestsController> _logger;

    public SomeDelegateTestsController(ILogger<SomeDelegateTestsController> logger)
    {
        _logger = logger;
    }
    
    [HttpGet("integer-test")]
    public ActionResult DoTest()
    {
        _logger.LogInformation($"Getting tests done");

        Repository<int> repo = new Repository<int>();
        repo.AddOptions(option =>
        {
            option.AddPolicy("MinimumValue", value => value > 5);
            option.AddPolicy("MaximumValue", value => value < 10);
        });

        repo.Add(1);
        repo.Add(3);
        repo.Add(6);
        repo.Add(9);
        repo.Add(12);
        repo.Add(15);


        return Ok(repo.GetAll());
    }
    
}