namespace dot_net_api.Dtos;

public class ClimbingGymDto
{
    public int Id { get; set; }

    public String Name { get; set; }
    
    public String Description { get; set; }
    
    public string City { get; set; }

    public string Street { get; set; }

    public string PostalCode { get; set; }    
    
    public List<ClimbingRouteDto> ClimbingRoutes { get; set; }
    
}