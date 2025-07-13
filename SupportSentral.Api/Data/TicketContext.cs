using Microsoft.EntityFrameworkCore;
using SupportSentral.Api.Entities;

namespace SupportSentral.Api.Data;

public class TicketContext (DbContextOptions<TicketContext> options) : DbContext(options)
{
    public DbSet<User> Tickets => Set<User>();
    
    
}