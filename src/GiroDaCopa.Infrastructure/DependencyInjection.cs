using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GiroDaCopa.Persistence.Context;

namespace GiroDaCopa.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<GiroDaCopaDbContext>(options =>
        {
            options.UseNpgsql(
                configuration.GetConnectionString("Default"));
        });

        return services;
    }
}