using AutoMapper;
using dot_net_api.Dtos;
using dot_net_api.Entities;
using dot_net_api.Exceptions;
using dot_net_api.Services;
using Microsoft.EntityFrameworkCore;

namespace dot_net_api.ClimbingGymService;

public class ClimbingGymService: IClimbingGymService
{
    private readonly ClimbingGymDbContext _dbContext;
    private readonly IMapper _mapper;

    public ClimbingGymService(ClimbingGymDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    public List<ClimbingGymDto> GetAll()
    {
        var climbingGyms = _dbContext.ClimbingGyms
            .Include(cg => cg.ClimbingRoutes)
            .Include(cg => cg.Address)
            .ToList();

        var dtos = _mapper.Map<List<ClimbingGymDto>>(climbingGyms);
        return dtos;
    }

    public ClimbingGymDto GetById(int id)
    {
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
        var climbingGym = _mapper.Map<ClimbingGym>(dto);
        var result = _dbContext.ClimbingGyms.Add(climbingGym);
        _dbContext.SaveChanges();
        Console.WriteLine($"Succesfully created climbing gym id: {climbingGym.Id}, result.Entity.Id: {result.Entity.Id}");
        return climbingGym.Id;
    }

    public void DeleteById(int id)
    {
        var climbingGym = _dbContext.ClimbingGyms.FirstOrDefault(cg => cg.Id == id);
        if (climbingGym is null)
        {
            throw new NotFoundException($"Climbing gym with id: {id} not found");
        }

        _dbContext.Remove(climbingGym);
        _dbContext.SaveChanges();
    }
}