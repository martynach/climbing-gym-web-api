using dot_net_api.Dtos;
using dot_net_api.Entities;
using dot_net_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace dot_net_api.Controllers;

[ApiController]
[Route("api/climbingGym")]
public class ClimbingGymController: ControllerBase
{
    private readonly IClimbingGymService _climbingGymService;
    
    public ClimbingGymController(IClimbingGymService climbingGymService)
    {
        _climbingGymService = climbingGymService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<ClimbingGym>> GetClimbingGyms()
    {
        var dtos = _climbingGymService.GetAll();
        return Ok(dtos);
    }

    [HttpGet("{climbingGymId}")]
    public ActionResult<ClimbingGym> GetClimbingGymById([FromRoute] int climbingGymId)
    {
        var dto = _climbingGymService.GetById(climbingGymId);
        return Ok(dto);
    }
    
    [HttpPost]
    public ActionResult AddClimbingGym([FromBody] CreateClimbingGymDto dto)
    {

        var id = _climbingGymService.AddNewClimbingGym(dto);
        return Created($"api/climbingGym/{id}", null);
    }

    [HttpDelete("{climbingGymId}")]
    public ActionResult DeleteById([FromRoute] int climbingGymId)
    {
        _climbingGymService.DeleteById(climbingGymId);
        return NoContent();
    }
}