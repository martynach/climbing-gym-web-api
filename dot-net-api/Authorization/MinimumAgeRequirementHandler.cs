using Microsoft.AspNetCore.Authorization;

namespace dot_net_api.Authorization;

public class MinimumAgeRequirementHandler: AuthorizationHandler<MinimumAgeRequirement>
{
    private readonly ILogger<MinimumAgeRequirementHandler> _logger;

    public MinimumAgeRequirementHandler(ILogger<MinimumAgeRequirementHandler> logger)
    {
        _logger = logger;
    }
    
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
    {
        _logger.LogInformation($"Starting to authorize user based on {Constants.AgeClaim} claim");
        var claim = context.User.Claims.FirstOrDefault(c => c.Type == Constants.AgeClaim);
        if (claim is null)
        {
            _logger.LogInformation($"Cannot authorize user because there is no {Constants.AgeClaim} claim");
            return Task.CompletedTask;
        }
        _logger.LogInformation($"{Constants.AgeClaim} claim value: {claim.Value}, required minimum value: {requirement.MinimumAge}");

        
        if (int.Parse(claim.Value) < requirement.MinimumAge)
        {
            _logger.LogInformation($"Cannot authorize user because user does not have required age");
            return Task.CompletedTask;
        }
        
        _logger.LogInformation($"Successfully authorized user");

        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}