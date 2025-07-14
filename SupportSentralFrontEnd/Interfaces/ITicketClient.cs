using SupportSentralFrontEnd.Models;

namespace SupportSentralFrontEnd.Interfaces;

public interface ITicketClient
{
    Task<List<Ticket>?> GetTicketAsync();
    void AddTicketAsync(TicketDetails ticket);
    Task<TicketDetails> GetTicketDetailsAsync(Guid? ticketId);
    Task UpdateTicketAsync(TicketDetails ticket, Guid? Id);
}