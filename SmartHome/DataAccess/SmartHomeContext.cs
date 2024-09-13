using Domain;
using IDomain;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class SmartHomeContext : DbContext
{
    public DbSet<Device> Devices { get; set; }
    
    public SmartHomeContext(DbContextOptions<SmartHomeContext> options) : base(options) {}

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