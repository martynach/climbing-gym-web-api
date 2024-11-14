using dot_net_api.Dtos;

namespace dot_net_api.Services;

public interface IClimbingRouteService
{
    public IEnumerable<ClimbingRouteDto> GetAll(int gymId);
    public ClimbingRouteDto GetById(int gymId, int routeId);

    public int AddNewClimbingRoute(int gymId, CreateClimbingRouteDto createRouteDto);

    public void RemoveAllRoutes(int gymId);
    public void RemoveById(int gymId, int routeId);
}