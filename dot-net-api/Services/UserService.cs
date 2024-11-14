using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using dot_net_api.Dtos;
using dot_net_api.Entities;
using dot_net_api.Exceptions;
using dot_net_api.Validators;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace dot_net_api.Services;

public class UserService: IUserService
{
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly ClimbingGymDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ILogger<UserService> _logger;
    private readonly AuthenticationSettings _authenticationSettings;

    public UserService(
        IPasswordHasher<User> passwordHasher,
        ClimbingGymDbContext dbContext,
        IMapper mapper,
        ILogger<UserService> logger,
        AuthenticationSettings authenticationSettings)
    {
        _passwordHasher = passwordHasher;
        _dbContext = dbContext;
        _mapper = mapper;
        _logger = logger;
        _authenticationSettings = authenticationSettings;
    }
    
    public void RegisterUser(RegisterUserDto dto)
    {
        _logger.LogInformation("Register user - start");
        var user = new User()
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            RoleId = dto.RoleId,
            Age = dto.Age
        };
        
        _logger.LogInformation("Register user - created entity");


        var hash = _passwordHasher.HashPassword(user, dto.Password);
        
        _logger.LogInformation("Register user - hashed password");

        user.HashedPassword = hash;
        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();
        
        _logger.LogInformation("Register user - finished successfully");
    }

    private string GenerateToken(User user)
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role.Name),
            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
            new Claim(ClaimTypes.Email, user.Email)
        };

        if (user.Age is not null)
        {
            
            var ageClaim = new Claim(Constants.AgeClaim, user.Age + "");
            claims.Add(ageClaim);
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);
        
        var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
            _authenticationSettings.JwtIssuer,
            claims,
            expires: expires,
            signingCredentials: credentials);

        var tokenHandler = new JwtSecurityTokenHandler();
        string stringToken = tokenHandler.WriteToken(token);
        return stringToken;
    }
    
    public string LoginUser(LoginUserDto dto)
    {
        
        _logger.LogInformation("Login user - start");

        var user = _dbContext
            .Users
            .Include(u => u.Role)
            .FirstOrDefault(u => u.Email == dto.Email);
        if (user is null)
        {
            throw new NotFoundException("User with given email does not exist");
        }

        _logger.LogInformation("Login user - successfully found user with given email");

        var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.HashedPassword, dto.Password);

        if (verificationResult == PasswordVerificationResult.Failed)
        {
            throw new NotFoundException("User with given password does not exist");
        }
        
        _logger.LogInformation("Login user - successfully verified user password");

        
        var token = GenerateToken(user);
        
        _logger.LogInformation("Login user - successfully generated user token");

        return token;
    }


    public IEnumerable<Role> GetRoles()
    {
        var roles = _dbContext.Roles.ToList();
        return roles;
    }

    public IEnumerable<GetUserDto> GetUsers()
    {
        var users = _dbContext.Users.Include(u => u.Role).ToList();
        var dtos = _mapper.Map<List<GetUserDto>>(users);
        return dtos;
    }

    public void DeleteById(int userId)
    {
        var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
        if (user is null)
        {
            throw new NotFoundException($"User with id: {userId} does not exist");
        }

        _dbContext.Users.Remove(user);
        _dbContext.SaveChanges();
    }

    public void DeleteAll()
    {
        var users = _dbContext.Users.ToList();
        _dbContext.Users.RemoveRange(users);
        _dbContext.SaveChanges();
    }
}