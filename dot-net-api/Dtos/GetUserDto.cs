using dot_net_api.Entities;

namespace dot_net_api.Dtos;

public class GetUserDto
{
    public int Id { get; set; }

    public string FirstName { get; set; }
    
    public string LastName { get; set; }

    public string Email { get; set; }
    
    public int RoleId { get; set; }
    
    public string Role { get; set; }
}