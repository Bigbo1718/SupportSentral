using Microsoft.EntityFrameworkCore;

namespace SupportSentral.Api.Data;

public static class DataExtension
{
    public static async Task MigrateDbAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<SupportContext>();
        await dbContext.Database.MigrateAsync();
        
    }
}