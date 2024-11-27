using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace dot_net_api.Entities;

public class ClimbingGymDbSeeder
{
    private readonly ClimbingGymDbContext _dbContext;
    private readonly IPasswordHasher<User> _passwordHasher;

    public ClimbingGymDbSeeder(ClimbingGymDbContext dbContext, IPasswordHasher<User> passwordHasher)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
    }

    public void Seed()
    {
        if (!_dbContext.Database.CanConnect())
        {
            Console.WriteLine("Cannot connect to db");
            return;
        }

        Console.WriteLine("Successfully connected to db");

        PerformPendingMigrations();
        SeedUserRoles();
        var userId = SeedAdminUser();
        SeedClimbingGyms(userId);
    }

    private void PerformPendingMigrations()
    {
        var pendingMigrations = _dbContext.Database.GetPendingMigrations();
        if(pendingMigrations != null && pendingMigrations.Any()) {
            Console.WriteLine("There are some pending migrations - performing migrations");
            _dbContext.Database.Migrate();
        }
        else
        {
            Console.WriteLine("There are no pending migrations - performing migration skipped");
        }
    }

    //returns id of admin user
    private int SeedAdminUser()
    {
        if (_dbContext.Users.IsNullOrEmpty())
        {
            var user = GetAdminUser();
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return user.Id;
        }
        
        var existingUser = _dbContext.Users.ToList().First();
        return existingUser.Id;

    }

    private void SeedClimbingGyms(int userId)
    {
        if (_dbContext.ClimbingGyms.IsNullOrEmpty())
        {
            Console.WriteLine("Climbing gyms are empty - start seeding");
            var gyms = GetInitialClimbingGyms(userId);
            _dbContext.ClimbingGyms.AddRange(gyms);
            _dbContext.SaveChanges();
        }
        else
        {
            Console.WriteLine("Climbing gyms seeding skipped.");
        }
    }

    private void SeedUserRoles()
    {
        if (_dbContext.Roles.IsNullOrEmpty())
        {
            Console.WriteLine("User roles are empty - start seeding");
            var roles = GetRoles();
            _dbContext.Roles.AddRange(roles);
            _dbContext.SaveChanges();
        }
        else
        {
            Console.WriteLine("User roles seeding skipped.");
        }
    }

    private User GetAdminUser()
    {
        var user = new User()
        {
            FirstName = "admin",
            LastName = "admin",
            Age = 40,
            RoleId = 3,
            Email = "admin@gmail.com",
        };
        
        var hashedPassword = _passwordHasher.HashPassword(user, "admin");

        user.HashedPassword = hashedPassword;
        return user;
    }

    private List<ClimbingGym> GetInitialClimbingGyms(int creatorId)
    {
        var climbingGyms = new List<ClimbingGym>()
        {
            new ClimbingGym()
            {
                Name = "Poog Baldy Climbing",
                CreatorId = creatorId,
                Description = "Good climbing on various different boulder problems",
                Address = new Address()
                {
                    City = "Krakow",
                    Street = "Glowackiego 15",
                    PostalCode = "45-543"
                },
                ClimbingRoutes = new List<ClimbingRoute>()
                {
                    new ClimbingRoute()
                        { Name = "warm me", Description = "easy warm up problem", Grade = 1, Status = "Old" },
                    new ClimbingRoute()
                        { Name = "Go very easy", Description = "easy warm up problem", Grade = 1, Status = "Active" },
                    new ClimbingRoute()
                        { Name = "No name", Description = "easy warm up problem", Grade = 1, Status = "Active" },
                    new ClimbingRoute()
                        { Name = "Go quite easy ", Description = "nice warm up problem", Grade = 2, Status = "Active" },
                    new ClimbingRoute()
                        { Name = "Go not so easy", Description = "intermediate problem", Grade = 3, Status = "Active" },
                    new ClimbingRoute()
                    {
                        Name = "Go crazy", Description = "quite powerful intermediate problem", Grade = 4,
                        Status = "Active"
                    },
                    new ClimbingRoute()
                    {
                        Name = "Go very crazy", Description = "powerful boulder with big holds", Grade = 5,
                        Status = "Active"
                    },
                    new ClimbingRoute()
                    {
                        Name = "Go for it or die for it",
                        Description = "only for best climbers, very powerful, long moves in the roof on small holds",
                        Grade = 6, Status = "Active"
                    }
                }
            },
            new ClimbingGym()
            {
                Name = "ForEver Climb&Eat",
                CreatorId = creatorId,
                Description = "Excellent climbs and even better food",
                Address = new Address()
                {
                    City = "Krakow",
                    Street = "Mysliwiecka 345",
                    PostalCode = "30-329"
                }
            }
        };
        return climbingGyms;
    }

    private List<Role> GetRoles()
    {
        return new List<Role>()
        {
            new Role() { Name = "User" },
            new Role() { Name = "Manager" },
            new Role() { Name = "Admin" }
        };
    }
}