using Microsoft.EntityFrameworkCore;
using SupportSentral.Api.Repositories;

namespace SupportSentral.Api.Data;

public static class DataExtension
{
    public static async Task MigrateDbAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<SupportContext>();
        await dbContext.Database.MigrateAsync();
        
    }
    public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
  
        var connectionString = configuration.GetConnectionString("SupportSentral");

        services.AddSqlite<SupportContext>(connectionString)
            .AddScoped<ITicketRepository, EntityFrameworkTicketRepository>()
            .AddScoped<IUserRepository, EntityFrameworkUserRepository>();

        return services;
    }
}