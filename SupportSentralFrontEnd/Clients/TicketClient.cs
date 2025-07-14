using SupportSentralFrontEnd.Mappers;
using SupportSentralFrontEnd.Models;

namespace SupportSentralFrontEnd.Clients;

public class TicketClient(HttpClient client)
{
    private readonly TicketMapper _ticketMapper = new();

    public async Task<List<Ticket>?> GetTicketAsync()
    {
        return await client.GetFromJsonAsync<List<Ticket>>("ticket");
    }
    
    public void AddTicketAsync(TicketDetails ticket)
    {
        if (ticket.StatusId != null)
        {
            var mappedTicket = _ticketMapper.MapFromTicketDetails(ticket);
            client.PostAsJsonAsync("/", mappedTicket);
        }
    }

    public async Task<TicketDetails> GetTicketDetailsAsync(Guid? ticketId)
    {
        Ticket? ticket= await client.GetFromJsonAsync<Ticket?>($"/ticket/{ticketId}");
        
        ArgumentNullException.ThrowIfNull(ticket);
        return await _ticketMapper.MapToTicketsDetails(ticket);
    }

    public async void  UpdateTicketAsync(TicketDetails ticket, Guid? Id)
    {
        Ticket? existingTicket = await GetTicketAsync(Id);
        ArgumentNullException.ThrowIfNull(existingTicket);
        existingTicket.Id = ticket.Id;
        existingTicket.Title = ticket.Title;
        existingTicket.Description = ticket.Description;
        existingTicket.UpdatedAt = DateTime.Now;
        ArgumentNullException.ThrowIfNull(ticket.StatusId);
        existingTicket.StatusId = int.Parse(ticket.StatusId);
    }

    private async Task<Ticket?> GetTicketAsync(Guid? Id)
    {
        return await client.GetFromJsonAsync<Ticket>($"tickets/{Id}");
    }
}