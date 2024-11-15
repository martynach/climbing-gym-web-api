using System.Security.Claims;
using dot_net_api.Entities;
using Microsoft.AspNetCore.Authorization;

namespace dot_net_api.Authorization;

public class CreatorRequirementHandler: AuthorizationHandler<CreatorRequirement, ClimbingGym>
{
    private readonly ILogger<CreatorRequirementHandler> _logger;

    public CreatorRequirementHandler(ILogger<CreatorRequirementHandler> logger)
    {
        _logger = logger;
    }
    
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CreatorRequirement requirement, ClimbingGym resource)
    {
        _logger.LogInformation($"Starting to authorize user based on resource creator");
        var claim = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

        if (claim is null)
        {
            _logger.LogInformation($"Cannot authorize user because there is no claim with name identifier");
            return Task.CompletedTask;
        }
        
        var creatorId = resource.CreatorId;
        if (creatorId is null)
        {
            _logger.LogInformation($"Cannot authorize user because there is no resource creator provided");
            return Task.CompletedTask;
        }

        _logger.LogInformation($"Authorizing user with name identifier {claim.Value} for access to resource with creator id: {creatorId}");

        
        if (int.Parse(claim.Value) != creatorId)
        {
            _logger.LogInformation($"Cannot authorize user because the user is not creator of the resource");
            return Task.CompletedTask;
        }
        
        _logger.LogInformation($"Successfully authorized user");

        context.Succeed(requirement);
        return Task.CompletedTask;
    }


}