using SupportSentralFrontEnd.Clients;
using SupportSentralFrontEnd.Interfaces;
using SupportSentralFrontEnd.Models;

namespace SupportSentralFrontEnd.Mappers;
public class TicketMapper
{
    private readonly TicketDetails _ticketsDetails;
    private readonly Ticket _tickets;
    private User[]? users;
    public IUserClient _userClient;
    private Status[]? statuses;
    public IStatusClient _statusClient;


    public async Task<Ticket?> MapFromTicketDetails(TicketDetails ticketsDetails)
    {

        var statusId = await _statusClient.GetStatusFromId(ticketsDetails.StatusId);
        if (ticketsDetails.UserId != null)
        {
            var user = await _userClient.GetUserFromId(ticketsDetails.UserId);
        
        
            if (ticketsDetails is { StatusId: not null, UserId: not null })
            {
                return new Ticket
                {
                    Title = ticketsDetails.Title,
                    Description = ticketsDetails.Description,
                    StatusId = ticketsDetails.StatusId,
                    CreatedAt = DateTime.UtcNow,
                    Id = ticketsDetails.Id,
                    AssignedTo = user!.Name,
                    UserId = user.Id,
                };
            }
        }

        return null;



    }

    public async Task<TicketDetails> MapToTicketsDetails(Ticket? ticket)
    {
        var user = await _userClient.GetUserFromEmail(ticket.AssignedTo);

        return new TicketDetails()
        {
            Title = ticket.Title,
            Description = ticket.Description,
            CreatedAt = ticket.CreatedAt,
            AssignToSelf = true, //Check if user is logged in user using UserClass, (Get CurrentUser)
            UserId = ticket.UserId,
            Id = ticket.Id,
            UpdatedAt = ticket.UpdatedAt,
            StatusId = ticket.StatusId

        };
    }
}