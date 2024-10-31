using BusinessLogic;
using DataAccess;
using IBusinessLogic;
using IDataAccess;
using ImportersLogic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceFactory;

public static class ServiceFactory
{
    public static void AddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped(typeof(IRepository<>), typeof(SqlRepository<>));
        serviceCollection.AddScoped<IDeviceService, DeviceService>();
        serviceCollection.AddScoped<IUserService, UserService>();
        serviceCollection.AddScoped<ISessionService, SessionService>();
        serviceCollection.AddScoped<IHomeService, HomeService>();
        serviceCollection.AddScoped<ICompanyService, CompanyService>();
        serviceCollection.AddScoped<INotificationService, NotificationService>();
        
        serviceCollection.AddScoped<IImporter.IImporter, ImporterLogic>();
    }
    
    public static void AddConnectionString(this IServiceCollection serviceCollection, string connectionString)
    {
        serviceCollection.AddDbContext<SmartHomeContext>
            (options => options.UseSqlServer
                (connectionString, b => b.MigrationsAssembly("DataAccess")));
    }
}