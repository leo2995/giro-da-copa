using GiroDaCopa.Application.Abstractions;
using GiroDaCopa.Domain.Entities;
using GiroDaCopa.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GiroDaCopa.Infrastructure.Seed;

public static class AdminSeeder
{
    public static async Task InitializeAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var context = scope.ServiceProvider
            .GetRequiredService<GiroDaCopaDbContext>();

        if (await context.Users.AnyAsync())
            return;

        var configuration = scope.ServiceProvider
            .GetRequiredService<IConfiguration>();

        var passwordHasher = scope.ServiceProvider
            .GetRequiredService<IPasswordHasher>();

        var username = configuration["Admin:Username"] ?? "admin";
        var password = configuration["Admin:Password"];

        if (string.IsNullOrWhiteSpace(password))
        {
            throw new InvalidOperationException(
                "Admin:Password must be configured via User Secrets, appsettings.Development.json or environment variables.");
        }

        var admin = new User(
            username,
            passwordHasher.Hash(password),
            "Admin");

        context.Users.Add(admin);
        await context.SaveChangesAsync();
    }
}
