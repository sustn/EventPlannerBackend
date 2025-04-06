using EventPlanner.Application.Interfaces;
using EventPlanner.Application.mappers;
using EventPlanner.Application.Services.EmailService;
using EventPlanner.Application.Services.EventService;
using Microsoft.Extensions.DependencyInjection;

namespace EventPlanner.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddAppplication(this IServiceCollection services) {
        services.ResolveServices();
        services.AddMappers();
        return services;
    }

    public static void ResolveServices(this IServiceCollection services) {
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IEmailService, EmailService>();
    }

    public static void AddMappers(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(EventMappingProfile).Assembly);
    }
}
