using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SupportSentral.Api.Data;

namespace SupportSentral.Api.IntegrationTests;

internal class SupportSentralWebApplicationFactory :  WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.AddSqlite<SupportContext>("Data Source=SupportSentral.db");
            var dbContext = CreateDbContext(services);
            dbContext.Database.EnsureDeleted();
        });
    }
    
    private static SupportContext CreateDbContext(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<SupportContext>();

        return dbContext;
    }
}