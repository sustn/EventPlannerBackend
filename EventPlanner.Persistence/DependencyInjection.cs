using EventPlanner.Application.Interfaces;
using EventPlanner.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventPlanner.Persistence;
public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDBContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDBContext).Assembly.FullName));
        });
        services.ResolveRepositories();
        return services;
    }
    public static void ResolveRepositories(this IServiceCollection services)
    {
        services.AddScoped<IRepository<Event>, Repository<Event>>();
        services.AddScoped<IRepository<Invite>, Repository<Invite>>();
        services.AddScoped<IRepository<Email>, Repository<Email>>();
    }
}
