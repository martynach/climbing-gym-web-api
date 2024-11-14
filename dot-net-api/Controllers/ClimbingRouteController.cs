using dot_net_api.Dtos;
using dot_net_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dot_net_api.Controllers;

[ApiController]
[Route("api/climbingGym/{gymId}/climbingRoute")]
[Authorize]
public class ClimbingRouteController: ControllerBase
{
    private readonly IClimbingRouteService _climbingRouteService;

    public ClimbingRouteController(IClimbingRouteService climbingRouteService)
    {
        _climbingRouteService = climbingRouteService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<ClimbingRouteDto>> GetAll([FromRoute] int gymId)
    {
        var dtos = _climbingRouteService.GetAll(gymId);
        return Ok(dtos);
    }
    
    [HttpGet("{routeId}")]
    public ActionResult<ClimbingRouteDto> GetById([FromRoute] int gymId, [FromRoute] int routeId)
    {
        var dto = _climbingRouteService.GetById(gymId, routeId);
        return Ok(dto);
    }
    
    [HttpPost]
    [Authorize(Policy = Constants.IsAdultPolicy)]
    public ActionResult<ClimbingRouteDto> Create([FromRoute] int gymId, [FromBody] CreateClimbingRouteDto dto)
    {
        var routeId = _climbingRouteService.AddNewClimbingRoute(gymId, dto);
        return Created($"api/climbingGym/{gymId}/climbingRoute/{routeId}", null); 
    }
    
    [HttpDelete]
    public ActionResult RemoveAllRoutesForGym([FromRoute] int gymId)
    {
        _climbingRouteService.RemoveAllRoutes(gymId);
        return NoContent();
    }
    
    [HttpDelete("{routeId}")]
    public ActionResult RemoveRouteForGym([FromRoute] int gymId, [FromRoute] int routeId)
    {
        _climbingRouteService.RemoveById(gymId, routeId);
        return NoContent();
    }
    
}