namespace dot_net_api.Entities;

public class ClimbingRoute
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int Grade { get; set; }

    public string Description { get; set; }
    
    public string Status { get; set; }
    
    public int ClimbingGymId { get; set; }

    // public virtual ClimbingGym ClimbingGym { get; set; }
}