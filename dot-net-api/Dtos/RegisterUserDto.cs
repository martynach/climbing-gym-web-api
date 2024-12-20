﻿
namespace dot_net_api.Dtos;

public class RegisterUserDto
{
    public string FirstName { get; set; }
    
    public string LastName { get; set; }

    public string Email { get; set; }
    
    public string ConfirmEmail { get; set; }

    public string Password { get; set; }
    
    public string ConfirmPassword { get; set; }
    
    public int RoleId { get; set; }
    
    public int? Age { get; set; }
    
}