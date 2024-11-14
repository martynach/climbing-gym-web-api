using Microsoft.IdentityModel.Tokens;

namespace dot_net_api.Entities;

public class ClimbingGymDbSeeder
{
    private readonly ClimbingGymDbContext _dbContext;

    public ClimbingGymDbSeeder(ClimbingGymDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Seed()
    {
        if (!_dbContext.Database.CanConnect())
        {
            Console.WriteLine("Cannot connect to db");
            return;
        }

        Console.WriteLine("Successfully connected to db");

        SeedClimbingGyms();
        SeedUserRoles();
    }

    private void SeedClimbingGyms()
    {
        if (_dbContext.ClimbingGyms.IsNullOrEmpty())
        {
            Console.WriteLine("Climbing gyms are empty - start seeding");
            var gyms = GetInitialClimbingGyms();
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

    private List<ClimbingGym> GetInitialClimbingGyms()
    {
        var climbingGyms = new List<ClimbingGym>()
        {
            new ClimbingGym()
            {
                Name = "Poog Baldy Climbing",
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