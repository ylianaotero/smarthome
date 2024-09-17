using Domain;
using IDomain;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class SmartHomeContext : DbContext
{
    public DbSet<Device> Devices { get; set; }
    public DbSet<HomeOwner> HomeOwners { get; set; }
    public DbSet<Administrator> Administrators { get; set; }
    public DbSet<CompanyOwner> CompanyOwners { get; set; }
    public DbSet<User> Users { get; set; }

    public SmartHomeContext(DbContextOptions<SmartHomeContext> options) : base(options)
    {
        this.Database.Migrate();
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(
                "Your_Connection_String_Here",
                b => b.MigrationsAssembly("DataAccess")); // Cambia "DataAccess" al nombre de tu proyecto de migraciones
        }
    }
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Device>().Property(d => d.Id).ValueGeneratedOnAdd();
        modelBuilder.Entity<SecurityCamera>().ToTable("Devices").HasBaseType<Device>();
        modelBuilder.Entity<WindowSensor>().ToTable("Devices").HasBaseType<Device>();
        modelBuilder.Entity<Device>()
            .HasDiscriminator<string>("DeviceType")
            .HasValue<SecurityCamera>("SecurityCamera")
            .HasValue<WindowSensor>("WindowSensor");
    }
}