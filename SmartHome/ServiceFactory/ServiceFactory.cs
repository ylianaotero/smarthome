using BusinessLogic;
using DataAccess;
using IBusinessLogic;
using IDataAccess;
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
    }
}