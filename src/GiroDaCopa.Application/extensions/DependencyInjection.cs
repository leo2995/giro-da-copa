using FluentValidation;
using GiroDaCopa.Application.Common.Behaviors;
using GiroDaCopa.Application.Common.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace GiroDaCopa.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(
                Assembly.GetExecutingAssembly());

            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(
            Assembly.GetExecutingAssembly());

        services.AddScoped<GroupStandingsService>();
        services.AddScoped<PoolScoringService>();

        return services;
    }
}