using dot_net_api.Dtos;
using dot_net_api.Entities;

namespace dot_net_api.Services;

public interface IUserService
{
    public void RegisterUser(RegisterUserDto dto);
    public string LoginUser(LoginUserDto dto);
    public IEnumerable<Role> GetRoles();
    public IEnumerable<GetUserDto> GetUsers();

    public void DeleteById(int userId);

    public void DeleteAll(

}