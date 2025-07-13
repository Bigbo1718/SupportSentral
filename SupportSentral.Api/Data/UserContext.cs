using Microsoft.EntityFrameworkCore;
using SupportSentral.Api.Entities;

namespace SupportSentral.Api.Data;

public class UserContext (DbContextOptions<UserContext>options): DbContext(options)
{

    public DbSet<Ticket> Tickets => Set<Ticket>();

    public DbSet<Status> Status => Set<Status>();

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Status>().HasData(
        new { Id = 1, Name = "New" },
        new { Id = 2, Name = "In Progress" },
        new { Id = 3, Name = "Closed" });
}
}