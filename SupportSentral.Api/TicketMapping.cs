using SupportSentral.Api.Contracts;
using SupportSentral.Api.Entities;

namespace SupportSentral.Api;

public static class TicketMapping
{
    public static Ticket ToEntity(this CreateTicketContract ticket)
    {
        return new Ticket()
        {
            Title = ticket.Title,
            Description = ticket.Description,
            StatusId = ticket.StatusId,
            CreatedDate = DateTime.Now,
            UserId = ticket.UserId
        };
    }
    
    public static Ticket ToEntity(this UpdateTicketDetailsContract ticket, Guid Id)
    {
        return new Ticket()
        {
            Id = Id,
            Title = ticket.Title,
            Description = ticket.Description,
            StatusId = ticket.StatusId,
            UpdatedDate = DateTime.UtcNow,
            UserId = ticket.UserId
        };
    }

    
    public static TicketContract ToContract(this Ticket ticket)
    {
        return new TicketContract(
                ticket.Id,
                ticket.Title,
                ticket.Description,
                ticket.Status!.Name,
                ticket.CreatedDate,
                ticket.UpdatedDate,
                ticket.UserId)
            ;
    }
    
    public static TicketDetailsContract ToTicketDetailsContract(this Ticket ticket)
    {
        return new TicketDetailsContract(
                ticket.Id,
                ticket.Title,
                ticket.Description,
                ticket.StatusId,
                ticket.CreatedDate,
                ticket.UpdatedDate,
                ticket.UserId)
            ;
    }
}