using dot_net_api.Dtos;
using dot_net_api.Entities;
using dot_net_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace dot_net_api.Controllers;


[ApiController]
[Route("auth")]
public class UserController: ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpPost("register")]
    public ActionResult RegisterUser([FromBody] RegisterUserDto dto)
    {
        _userService.RegisterUser(dto);
        return Ok();
    }
    
    [HttpPost("login")]
    public ActionResult<string> LoginUser([FromBody] LoginUserDto dto)
    {
        var token = _userService.LoginUser(dto);
        return Ok(token);
    }
    
    [HttpGet("roles")]
    public ActionResult<Role> GetRoles()
    {
        var roles = _userService.GetRoles();
        return Ok(roles);
    }
    
    [HttpGet("users")]
    public ActionResult<GetUserDto> GetUsers()
    {
        var users = _userService.GetUsers();
        return Ok(users);
    }
    
    [HttpDelete("users/{userId}")]
    public ActionResult DeleteById([FromRoute] int userId)
    {
        _userService.DeleteById(userId);
        return NoContent();
    }
    
    [HttpDelete("users/all")]
    public ActionResult DeleteAll()
    {
        _userService.DeleteAll();
        return NoContent();
    }
    
}