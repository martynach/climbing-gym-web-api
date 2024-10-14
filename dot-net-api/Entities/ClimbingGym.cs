namespace dot_net_api.Entities;

public class ClimbingGym
{
    public int Id { get; set; }

    public String Name { get; set; }
    
    public String Description { get; set; }
    
    public int AddressId { get; set; }
    
    public virtual Address Address { get; set; }
    
    public virtual List<ClimbingRoute> ClimbingRoutes { get; set; }
}

