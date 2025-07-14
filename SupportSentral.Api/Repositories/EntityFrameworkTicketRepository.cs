using Microsoft.EntityFrameworkCore;
using SupportSentral.Api.Contracts;
using SupportSentral.Api.Data;
using SupportSentral.Api.Mappings;

namespace SupportSentral.Api.Repositories;

public class EntityFrameworkTicketRepository : ITicketRepository
{
    private readonly SupportContext _dbContext;

    public EntityFrameworkTicketRepository(SupportContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<TicketContract>> GetAllAsync()
    {
        return await _dbContext.Tickets
            .Include((ticket => ticket.Status))
            .Select(ticket => ticket.ToContract())
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<TicketDetailsContract?> GetByIdAsync(Guid id)
    {
        var ticket = await _dbContext.Tickets.FindAsync(id);

        return ticket?.ToTicketDetailsContract();
    }
    
    
    public async Task<TicketDetailsContract?> CreateTicketAsync(CreateTicketContract ticket)
    {
        var isValidTicket = ValidateCreatedTicket(ticket);

        if (!isValidTicket)
        {
            return null;
        }
        var createdTicketEntity = _dbContext.Tickets.Add(ticket.ToEntity());
        await _dbContext.SaveChangesAsync();
        var ticketDetails = createdTicketEntity.Entity.ToTicketDetailsContract();
        return ticketDetails;
    }

    private static bool ValidateCreatedTicket(CreateTicketContract ticket)
    {
        if (ticket.UserId.Equals(Guid.Empty))
        {
            return false;
        };

        return !ticket.StatusId.Equals(0);
    }

    public async Task<bool>UpdateTicketDetailsContract (UpdateTicketDetailsContract ticket, Guid id)
    {
        
        var existingTicket = await _dbContext.Tickets.FindAsync(id);
        if (existingTicket == null)
        {
            return false;
        }
        if (ticket.StatusId.Equals(0))
        {
            return false;
        }
            
        _dbContext.Entry(existingTicket)
            .CurrentValues
            .SetValues(ticket.ToEntity(id));
       await _dbContext.SaveChangesAsync();

       return true;
    }
}