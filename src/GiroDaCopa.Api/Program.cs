using GiroDaCopa.Api.Swagger;
using GiroDaCopa.Application;
using GiroDaCopa.Infrastructure;
using GiroDaCopa.Infrastructure.Seed;
using GiroDaCopa.Persistence.Context;
using GiroDaCopa.Persistence.Seed;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(
    builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithJwt();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<GiroDaCopaDbContext>();
    await db.Database.MigrateAsync();
}

await SeedData.InitializeAsync(app.Services);
await AdminSeeder.InitializeAsync(app.Services);

app.Run();