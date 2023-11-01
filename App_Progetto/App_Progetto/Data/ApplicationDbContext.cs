using App_Progetto.Models;
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

    public DbSet<Attuatore> Attuatores => Set<Attuatore>();

    public DbSet<Sensore> Sensores => Set<Sensore>();

    public DbSet<Terreno> Terrenos => Set<Terreno>();

    public DbSet<Gestione> Gestiones => Set<Gestione>();

    public DbSet<Misurazione> Misuraziones => Set<Misurazione>();

    public DbSet<Piano> Pianos => Set<Piano>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        //seedRoles(modelBuilder);

        #region Terreno
        modelBuilder.Entity<Terreno>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.ToTable("Terreno");
        });
        #endregion

        #region Misurazione
        // Configura la chiave primaria per la classe Misurazione
        modelBuilder.Entity<Misurazione>(entity =>
        {
            entity.HasKey(m => new { m.DataOra, m.CodiceSensore });
            entity.ToTable("Misurazione");
            /*entity.HasOne(m => m.Sensores)
            .WithOne(s => s.Misuraziones)
            .HasForeignKey<Misurazione>(m => m.CodiceSensore)
            .OnDelete(DeleteBehavior.Cascade);
            */
            entity.HasOne(m => m.Sensores)
            .WithMany(s => s.Misuraziones)
            .HasForeignKey(m => m.CodiceSensore)
            .OnDelete(DeleteBehavior.Cascade);

        });
        #endregion

        #region Piano
        modelBuilder.Entity<Piano>(entity =>
        {
            entity.HasKey(p => p.CodicePiano);
            entity.ToTable("Piano");

            entity.HasOne(p => p.Attuatores)
            .WithOne(a => a.Pianos)
            .HasForeignKey<Piano>(p => p.CodAtt);
        });
        #endregion

        #region Sensore
        // Configura la relazione uno-a-uno con la chiave esterna
        modelBuilder.Entity<Sensore>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.ToTable("Sensore");
            entity.HasOne(s => s.Terreno)
            .WithMany(t => t.Sensores)
            .HasForeignKey(s => s.TerrenoId)
            .OnDelete(DeleteBehavior.Cascade);

        });
        #endregion

        #region Attuatore
        modelBuilder.Entity<Attuatore>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.ToTable("Attuatore");
            entity.HasOne(s => s.Terreno)
            .WithMany(t => t.Attuatores)
            .HasForeignKey(s => s.TerrenoId)
            .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(s => s.Pianos)
            .WithOne(p => p.Attuatores)
            .HasForeignKey<Piano>(p => p.CodAtt)
            .IsRequired();

        });
        #endregion

        #region Gestione
        modelBuilder.Entity<Gestione>(entity =>
        {
            entity.HasKey(g => g.Id);
            entity.ToTable("Gestione");
        });
        #endregion

        base.OnModelCreating(modelBuilder);
    }


    /*private static void seedRoles(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole() { Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "ADMIN" },
            new IdentityRole() { Name = "Agricoltore", ConcurrencyStamp = "2", NormalizedName = "AGRICOLTORE" },
            new IdentityRole() { Name = "Collaboratore", ConcurrencyStamp = "3", NormalizedName = "COLLABORATORE" }
       );
    }*/
}