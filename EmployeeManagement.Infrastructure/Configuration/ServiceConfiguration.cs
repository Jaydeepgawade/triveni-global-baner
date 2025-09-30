using EmployeeManagement.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeManagement.Infrastructure.Configuration;

public static class ServiceConfiguration
{
    public static IServiceCollection AddInfrastructureServicesConfiguration(this IServiceCollection services)
    {
        services.AddScoped<IDatabaseService, DatabaseService>();
        return services;
    }
}
