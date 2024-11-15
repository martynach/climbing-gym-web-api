using System.Security.Claims;

namespace dot_net_api.Services;

public interface IUserContextService
{
    public ClaimsPrincipal? User { get; } 
    
    public int? UserId { get; }
    
}