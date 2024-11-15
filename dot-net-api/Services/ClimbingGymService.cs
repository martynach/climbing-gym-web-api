using System.Security.Claims;
using AutoMapper;
using dot_net_api.Authorization;
using dot_net_api.Dtos;
using dot_net_api.Entities;
using dot_net_api.Exceptions;
using dot_net_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace dot_net_api.ClimbingGymService;

public class ClimbingGymService: IClimbingGymService
{
    private readonly ClimbingGymDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ILogger<ClimbingRouteService> _logger;
    private readonly IUserContextService _userContextService;
    private readonly IAuthorizationService _authorizationService;

    public ClimbingGymService(ClimbingGymDbContext dbContext,
        IMapper mapper,
        ILogger<ClimbingRouteService> logger,
        IUserContextService userContextService,
        IAuthorizationService authorizationService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _logger = logger;
        _userContextService = userContextService;
        _authorizationService = authorizationService;
    }
    
    public List<ClimbingGymDto> GetAll()
    {
        _logger.LogInformation("Getting all climbing gyms");
        var climbingGyms = _dbContext.ClimbingGyms
            .Include(cg => cg.ClimbingRoutes)
            .Include(cg => cg.Address)
            .ToList();

        var dtos = _mapper.Map<List<ClimbingGymDto>>(climbingGyms);
        return dtos;
    }

    public ClimbingGymDto GetById(int id)
    {
        _logger.LogInformation($"Getting climbing gym with id: {id}");

        var climbingGym = _dbContext.ClimbingGyms
            .Include(cg => cg.ClimbingRoutes)
            .Include(cg => cg.Address)
            .FirstOrDefault(cg => cg.Id == id);

        if (climbingGym is null)
        {
            throw new NotFoundException($"Climbing gym with id: {id} not found");
        }

        var dto = _mapper.Map<ClimbingGymDto>(climbingGym);
        return dto;
    }

    public int AddNewClimbingGym(CreateClimbingGymDto dto)
    {
        _logger.LogInformation($"Creating new climbing gym by user with id: {_userContextService.UserId}");

        var climbingGym = _mapper.Map<ClimbingGym>(dto);
        climbingGym.CreatorId = _userContextService.UserId;
        var result = _dbContext.ClimbingGyms.Add(climbingGym);
        _dbContext.SaveChanges();
        _logger.LogInformation($"Successfully created climbing gym with id: {climbingGym.Id}, result.Entity.Id: {result.Entity.Id}");
        return climbingGym.Id;
    }

    public void DeleteById(int id)
    {
        _logger.LogInformation($"Deleting climbing gym with id: {id}");

        var climbingGym = _dbContext.ClimbingGyms.FirstOrDefault(cg => cg.Id == id);
        if (climbingGym is null)
        {
            throw new NotFoundException($"Climbing gym with id: {id} not found");
        }
        
        ClaimsPrincipal? user = _userContextService.User;
        if (user is null)
        {
            throw new ForbidException("User is required (Authorization header)");
        }

        var authorizationResult =
            _authorizationService.AuthorizeAsync(user, climbingGym, new CreatorRequirement()).Result;

        if (!authorizationResult.Succeeded)
        {
            throw new ForbidException($"User with id: {_userContextService.UserId} cannot delete restaurant created by user with id: {climbingGym.CreatorId ?? -1}");
        }
        

        _dbContext.Remove(climbingGym);
        _dbContext.SaveChanges();
    }
}