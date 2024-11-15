using System.Security.Claims;

namespace dot_net_api.Services;

public class UserContextService: IUserContextService
{
    private readonly IHttpContextAccessor _httpContext;
    public ClaimsPrincipal? User => _httpContext.HttpContext?.User;
    
    private Claim? UserIdClaim => User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
    
    public int? UserId => UserIdClaim is null ? null : int.Parse(UserIdClaim.Value);


    public UserContextService(IHttpContextAccessor httpContext)
    {
        _httpContext = httpContext;

    }
}