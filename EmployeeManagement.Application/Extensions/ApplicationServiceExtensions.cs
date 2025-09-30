using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Application.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeManagement.Application.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<ISalaryService, SalaryService>();

        return services;
    }

    public static IServiceCollection AddApplicationValidators(this IServiceCollection services)
    {
        // Register FluentValidation validators
        services.AddValidatorsFromAssemblyContaining<EmployeeManagement.Application.Validators.CreateEmployeeDtoValidator>();
        
        return services;
    }
}
