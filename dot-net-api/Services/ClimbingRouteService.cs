using AutoMapper;
using dot_net_api.Dtos;
using dot_net_api.Entities;
using dot_net_api.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace dot_net_api.Services;

public class ClimbingRouteService: IClimbingRouteService
{
    private readonly ClimbingGymDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<ClimbingRouteService> _logger;

    public ClimbingRouteService(ClimbingGymDbContext context, IMapper mapper, ILogger<ClimbingRouteService> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }
    
    public IEnumerable<ClimbingRouteDto> GetAll(int gymId)
    {
        _logger.LogInformation($"Getting all climbing routes for gym with id: {gymId}");

        var climbingGym = getClimbingGymById(gymId);

        var dtos = _mapper.Map<List<ClimbingRouteDto>>(climbingGym.ClimbingRoutes);
        return dtos;
    }
    
    public ClimbingRouteDto GetById(int gymId, int routeId)
    {
        _logger.LogInformation($"Getting climbing route with id: {routeId} for gym with id: {gymId}");

        getClimbingGymById(gymId);

        var climbingRoute = _context.ClimbingRoutes.FirstOrDefault(r => r.Id == routeId);

        if (climbingRoute is null)
        {
            throw new NotFoundException($"Climbing route with id: {routeId} does not exist");
        }

        if (climbingRoute.ClimbingGymId != gymId)
        {
            throw new NotFoundException($"Climbing route with id: {routeId} does not exist for gym with id: {gymId}");

        }

        var dto = _mapper.Map<ClimbingRouteDto>(climbingRoute);
        return dto;
    }


    public int AddNewClimbingRoute(int gymId, CreateClimbingRouteDto createRouteDto)
    {
        _logger.LogInformation($"Adding new climbing route for gym with id: {gymId}");

        var climbingGym = getClimbingGymById(gymId);

        var climbingRoute = _mapper.Map<ClimbingRoute>(createRouteDto);
        climbingRoute.ClimbingGymId = gymId;
        
        // todo 1 check what if climbing route has no gym id provided
        // todo 2 check what is climbing route has gym id not existing

        _context.ClimbingRoutes.Add(climbingRoute);
        _context.SaveChanges();
        _logger.LogInformation($"Successfully created climbing route with id: {climbingRoute.Id}, for gym with id: {climbingGym.Id}");

        return climbingRoute.Id;
    }

    public void RemoveAllRoutes(int gymId)
    {
        _logger.LogInformation($"Deleting all climbing routes for gym with id: {gymId}");

        var climbingGym = getClimbingGymById(gymId);
        
        _context.ClimbingRoutes.RemoveRange(climbingGym.ClimbingRoutes);
        _context.SaveChanges();
    }
    
    public void RemoveById(int gymId, int routeId)
    {
        _logger.LogInformation($"Deleting climbing route with id: {routeId} for gym with id: {gymId}");

        var climbingGym = getClimbingGymById(gymId);
        
        var climbingRoute = _context.ClimbingRoutes.FirstOrDefault(r => r.Id == routeId);

        if (climbingRoute is null)
        {
            throw new NotFoundException($"Climbing route with id: {routeId} does not exist");
        }  
        
        if (climbingRoute.ClimbingGymId != gymId)
        {
            throw new NotFoundException($"Climbing route with id: {routeId} does not exist for gym with id: {gymId}");
        }

        _context.ClimbingRoutes.Remove(climbingRoute);
        
        _context.SaveChanges();
    }

    private ClimbingGym getClimbingGymById(int gymId)
    {
        var climbingGym = _context.ClimbingGyms
            .Include(g => g.ClimbingRoutes)
            .FirstOrDefault(g => g.Id == gymId);
        
        if (climbingGym is null)
        {
            throw new NotFoundException($"Climbing gym with id: {gymId} not found");
        }

        return climbingGym;
    }
}