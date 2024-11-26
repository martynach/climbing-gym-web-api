using System.Security.Claims;
using dot_net_api.Entities;
using Microsoft.AspNetCore.Authorization;

namespace dot_net_api.Authorization;

public class CreatedManyResourcesRequirementHandler: AuthorizationHandler<CreatedManyResourcesRequirement>
{
    private readonly ILogger<CreatedManyResourcesRequirementHandler> _logger;
    private readonly ClimbingGymDbContext _gymDbContext;

    public CreatedManyResourcesRequirementHandler(ILogger<CreatedManyResourcesRequirementHandler> logger, ClimbingGymDbContext gymDbContext)
    {
        _logger = logger;
        _gymDbContext = gymDbContext;
    }
    
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CreatedManyResourcesRequirement requirement)
    {
        _logger.LogInformation(
            $"Authorizing user by number of created resources; required: {requirement.MinimumAmountRequired};");


        var stringUserId = context.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
        if (stringUserId is null)
        {
            return Task.CompletedTask;
        }
        
        var userId = int.Parse(stringUserId);

        var numOfCreatedResources = _gymDbContext.ClimbingGyms.Count(g => g.CreatorId == userId);
        
        _logger.LogInformation(
            $"Authorizing user by number of created resources; actual number of created resources: {numOfCreatedResources};");
        
        if (requirement.MinimumAmountRequired <= numOfCreatedResources)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}