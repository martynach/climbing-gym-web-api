namespace dot_net_api.Entities;

public class User
{
    public int Id { get; set; }

    public string FirstName { get; set; }
    
    public string LastName { get; set; }

    public string Email { get; set; }

    public string HashedPassword { get; set; }
    
    public int RoleId { get; set; }
    
    public virtual Role Role { get; set; }
    
    public int? Age { get; set; }
}