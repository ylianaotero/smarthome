using Domain.Abstract;
using Domain.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DataAccess
{
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

        private const string DeviceTableName = "Devices";
        private const string RolesTableName = "Roles";
        private const string HomesTableName = "Homes";
        private const string DiscriminatorColumnName = "Kind";
        private const string FunctionalitiesColumnName = "Functionalities";
        private const string SecurityCameraDiscriminator = "SecurityCamera";
        private const string WindowSensorDiscriminator = "WindowSensor";
        private const string MotionSensorDiscriminator = "MotionSensor";
        private const string SmartLampDiscriminator = "SmartLamp";
        private const string AdministratorDiscriminator = "Administrator";
        private const string HomeOwnerDiscriminator = "HomeOwner";
        private const string CompanyOwnerDiscriminator = "CompanyOwner";

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
            ConfigureDevice(modelBuilder);
            ConfigureDeviceUnit(modelBuilder);
            ConfigureUser(modelBuilder);
            ConfigureRoles(modelBuilder);
            ConfigureCompany(modelBuilder);
            ConfigureNotification(modelBuilder);
            ConfigureHome(modelBuilder);
            ConfigureMember(modelBuilder);
            ConfigureRoom(modelBuilder);
        }

        private void ConfigureDevice(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Device>().Property(d => d.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Device>().HasOne<Company>(d => d.Company);
            modelBuilder.Entity<Device>().Navigation(d => d.Company).AutoInclude();
            modelBuilder.Entity<Device>()
                .HasDiscriminator<string>(DiscriminatorColumnName)
                .HasValue<SecurityCamera>(SecurityCameraDiscriminator)
                .HasValue<WindowSensor>(WindowSensorDiscriminator)
                .HasValue<MotionSensor>(MotionSensorDiscriminator)
                .HasValue<SmartLamp>(SmartLampDiscriminator);

            modelBuilder.Entity<SecurityCamera>().ToTable(DeviceTableName).HasBaseType<Device>();
            modelBuilder.Entity<SecurityCamera>()
                .Property(sc => sc.Functionalities)
                .HasColumnName(FunctionalitiesColumnName);

            modelBuilder.Entity<WindowSensor>().ToTable(DeviceTableName).HasBaseType<Device>();
            modelBuilder.Entity<WindowSensor>()
                .Property(ws => ws.Functionalities)
                .HasColumnName(FunctionalitiesColumnName);

            modelBuilder.Entity<MotionSensor>().ToTable(DeviceTableName).HasBaseType<Device>();
            modelBuilder.Entity<MotionSensor>()
                .Property(ms => ms.Functionalities)
                .HasColumnName(FunctionalitiesColumnName);

            modelBuilder.Entity<SmartLamp>().ToTable(DeviceTableName).HasBaseType<Device>();
            modelBuilder.Entity<SmartLamp>()
                .Property(sl => sl.Functionalities)
                .HasColumnName(FunctionalitiesColumnName);
        }

        private void ConfigureDeviceUnit(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DeviceUnit>().HasOne<Device>(du => du.Device);
            modelBuilder.Entity<DeviceUnit>().Navigation(du => du.Device).AutoInclude();

            modelBuilder.Entity<DeviceUnit>().HasOne<Room>(du => du.Room)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DeviceUnit>().Navigation(du => du.Room).AutoInclude();
        }

        private void ConfigureUser(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasMany<Role>(u => u.Roles);
            modelBuilder.Entity<User>().Navigation(r => r.Roles).AutoInclude();
        }

        private void ConfigureRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().Property(r => r.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Role>().HasDiscriminator<string>(DiscriminatorColumnName)
                .HasValue<Administrator>(AdministratorDiscriminator)
                .HasValue<HomeOwner>(HomeOwnerDiscriminator)
                .HasValue<CompanyOwner>(CompanyOwnerDiscriminator);

            modelBuilder.Entity<Administrator>().ToTable(RolesTableName).HasBaseType<Role>();
            modelBuilder.Entity<HomeOwner>().ToTable(RolesTableName).HasBaseType<Role>();
            modelBuilder.Entity<CompanyOwner>().ToTable(RolesTableName).HasBaseType<Role>();
        }

        private void ConfigureCompany(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>().Property(c => c.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Company>().HasOne<User>(c => c.Owner);
            modelBuilder.Entity<Company>().Navigation(c => c.Owner).AutoInclude();
        }

        private void ConfigureNotification(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Member)
                .WithMany(u => u.Notifications)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Notification>().Navigation(n => n.Member).AutoInclude();
            modelBuilder.Entity<Notification>().Navigation(n => n.DeviceUnitService).AutoInclude();
            modelBuilder.Entity<Notification>().Navigation(n => n.Home).AutoInclude();
        }

        private void ConfigureHome(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Home>().Navigation(h => h.Members).AutoInclude();
            modelBuilder.Entity<Home>().Navigation(h => h.Devices).AutoInclude();
            modelBuilder.Entity<Home>().Navigation(h => h.Owner).AutoInclude();
            modelBuilder.Entity<Home>().Navigation(h => h.Rooms).AutoInclude();
            modelBuilder.Entity<Home>().ToTable(HomesTableName);
            modelBuilder.Entity<Home>().Property(h => h.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Home>().HasOne<User>(h => h.Owner);
            modelBuilder.Entity<Home>().HasMany<Member>(h => h.Members);
            modelBuilder.Entity<Home>().HasMany<DeviceUnit>(h => h.Devices);
        }

        private void ConfigureMember(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Member>().Navigation(m => m.User).AutoInclude();
        }

        private void ConfigureRoom(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Room>().Property(r => r.Id).ValueGeneratedOnAdd();
        }
    }
}
