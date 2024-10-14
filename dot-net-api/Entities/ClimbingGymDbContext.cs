using Microsoft.EntityFrameworkCore;

namespace dot_net_api.Entities;

public class ClimbingGymDbContext: DbContext
{
    
    public DbSet<ClimbingGym> ClimbingGyms { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<ClimbingRoute> ClimbingRoutes { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<ClimbingGym>()
            .Property(cg => cg.Name)
            .IsRequired();
        
        modelBuilder.Entity<Address>()
            .Property(a => a.PostalCode)
            .IsRequired();
        
        modelBuilder.Entity<Address>()
            .Property(a => a.Street)
            .IsRequired();
        
        modelBuilder.Entity<Address>()
            .Property(a => a.City)
            .IsRequired();

        modelBuilder.Entity<ClimbingRoute>()
            .Property(r => r.Name)
            .IsRequired();
        
        modelBuilder.Entity<ClimbingRoute>()
            .Property(r => r.Grade)
            .IsRequired();
        
        modelBuilder.Entity<ClimbingRoute>()
            .Property(r => r.Status)
            .IsRequired();

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // todo
        string connectionString = "todo";
        optionsBuilder.UseSqlServer(connectionString);

    }

}