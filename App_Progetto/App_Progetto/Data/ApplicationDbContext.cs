using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace App_Progetto.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        seedRoles(modelBuilder);
    }

    private static void seedRoles(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole() { Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "ADMIN" },
            new IdentityRole() { Name = "Agricoltore", ConcurrencyStamp = "2", NormalizedName = "AGRICOLTORE" },
            new IdentityRole() { Name = "Collaboratore", ConcurrencyStamp = "3", NormalizedName = "COLLABORATORE" }
       );
    }
}