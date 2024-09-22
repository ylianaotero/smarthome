using BusinessLogic;
using BusinessLogic.Services;
using IBusinessLogic;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceFactory;

public static class ServiceFactory
{
    public static void AddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ISessionService, SessionService>();
        serviceCollection.AddScoped<IUserService, UserService>();
        serviceCollection.AddScoped<IDeviceService, DeviceService>();
    }
}