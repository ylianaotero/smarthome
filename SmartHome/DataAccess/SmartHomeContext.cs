using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DataAccess;

public class SmartHomeContext : DbContext
{
    public DbSet<Device> Devices { get; set; }
    public DbSet<DeviceUnit> DeviceUnits { get; set; }
    public DbSet<HomeOwner> HomeOwners { get; set; }
    public DbSet<Administrator> Administrators { get; set; }
    public DbSet<CompanyOwner> CompanyOwners { get; set; }
    
    public DbSet<Session> Sessions { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Member> Members { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Home> Homes { get; set; }
    
    public SmartHomeContext(DbContextOptions<SmartHomeContext> options, bool useInMemoryDatabase) : base(options)
    {
        if (!useInMemoryDatabase)
        {
            this.Database.Migrate();
        }
    }
    
    public SmartHomeContext(DbContextOptions<SmartHomeContext> options) : base(options)
    {

        this.Database.Migrate();
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.DetachedLazyLoadingWarning)); 
        }
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Device>().Property(d => d.Id).ValueGeneratedOnAdd();
        modelBuilder.Entity<SecurityCamera>().ToTable("Devices").HasBaseType<Device>();
        modelBuilder.Entity<WindowSensor>().ToTable("Devices").HasBaseType<Device>();
        
        modelBuilder.Entity<WindowSensor>().Property(ws => ws.Functionalities).HasColumnName("Functionalities");
        modelBuilder.Entity<SecurityCamera>().Property(sc => sc.Functionalities).HasColumnName("Functionalities");
            
        modelBuilder.Entity<Device>()
            .HasDiscriminator<string>("Kind")
            .HasValue<SecurityCamera>("SecurityCamera")
            .HasValue<WindowSensor>("WindowSensor");
        modelBuilder.Entity<DeviceUnit>().HasOne<Device>(du => du.Device);
        
        modelBuilder.Entity<Device>().HasOne<Company>(d=>d.Company);
        modelBuilder.Entity<Company>().Property(c => c.Id).ValueGeneratedOnAdd();
        modelBuilder.Entity<Company>().HasOne<User>(c => c.Owner);
        modelBuilder.Entity<Notification>()
            .HasOne(n => n.Member)
            .WithMany(u => u.Notifications)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<User>().HasMany<Role>(u => u.Roles);
        
        modelBuilder.Entity<Administrator>().ToTable("Roles").HasBaseType<Role>();
        modelBuilder.Entity<HomeOwner>().ToTable("Roles").HasBaseType<Role>();
        modelBuilder.Entity<CompanyOwner>().ToTable("Roles").HasBaseType<Role>();
        
        modelBuilder.Entity<Role>().Property(r => r.Id).ValueGeneratedOnAdd();
        modelBuilder.Entity<Role>().HasDiscriminator<string>("Kind")
            .HasValue<Administrator>("Administrator")
            .HasValue<HomeOwner>("HomeOwner")
            .HasValue<CompanyOwner>("CompanyOwner");
        
        modelBuilder.Entity<User>().Navigation(r => r.Roles).AutoInclude();
        modelBuilder.Entity<Home>().Navigation(h => h.Members).AutoInclude();
        modelBuilder.Entity<Home>().Navigation(h => h.Devices).AutoInclude();
        modelBuilder.Entity<Home>().Navigation(h => h.Owner).AutoInclude();
        modelBuilder.Entity<Member>().Navigation(m => m.User).AutoInclude();
        modelBuilder.Entity<Device>().Navigation(d => d.Company).AutoInclude();
        modelBuilder.Entity<Member>().Navigation(m => m.Notifications).AutoInclude();
        modelBuilder.Entity<Notification>().Navigation(n => n.Member).AutoInclude();
        modelBuilder.Entity<DeviceUnit>().Navigation(du => du.Device).AutoInclude();
        modelBuilder.Entity<Device>().Navigation(d => d.Company).AutoInclude();
        modelBuilder.Entity<CompanyOwner>().Navigation(co => co.Company).AutoInclude();

        modelBuilder.Entity<Home>().ToTable("Homes");
        modelBuilder.Entity<Home>().Property(h => h.Id).ValueGeneratedOnAdd();
        modelBuilder.Entity<Home>().HasOne<User>(h => h.Owner);
        modelBuilder.Entity<Home>().HasMany<Member>(h => h.Members);
        modelBuilder.Entity<Home>().HasMany<DeviceUnit>(h => h.Devices);
    }
}