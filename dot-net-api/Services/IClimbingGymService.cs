using dot_net_api.Dtos;

namespace dot_net_api.Services;

public interface IClimbingGymService
{
    public List<ClimbingGymDto> GetAll();
    public ClimbingGymDto GetById(int id);
    public int AddNewClimbingGym(CreateClimbingGymDto dto);
    public void DeleteById(int id);
}