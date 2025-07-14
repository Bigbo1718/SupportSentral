using Microsoft.EntityFrameworkCore;
using SupportSentral.Api.Contracts;
using SupportSentral.Api.Data;
using SupportSentral.Api.Mappings;
using SupportSentral.Api.Repositories;

namespace SupportSentral.Api.Endpoints;

public static class TicketEndpoints 
{
     public static RouteGroupBuilder MapToTicketEndpoint(this WebApplication app)
    {
        var group = app.MapGroup("tickets")
            .WithParameterValidation();

        const string getTicketEndpointName = "GetTicket";
        
        //Get /tickets
        group.MapGet("/", async (ITicketRepository repository) => (
            await repository.GetAllAsync())
        );
        
        //GET tickets/id
        group.MapGet("/{id}", async (Guid id, ITicketRepository repository) =>
        {
            TicketDetailsContract? ticket = await repository.GetByIdAsync(id);
            
            return ticket != null ? 
                Results.Ok(ticket) : Results.NotFound();
            
        }).WithName(getTicketEndpointName);
        
        //Post tickets/
        group.MapPost("/", async (CreateTicketContract ticket, ITicketRepository repository) =>
        {
            TicketDetailsContract? ticketDetails = await repository.CreateTicketAsync(ticket);
            
            if (ticketDetails != null)
              return Results.CreatedAtRoute(getTicketEndpointName,
                new { ticketDetails.Id}, ticketDetails);
            
            return Results.BadRequest();
        }).WithParameterValidation();; 
        
        //Put tickets
        group.MapPut("/{id}", async (Guid id, UpdateTicketDetailsContract ticket, ITicketRepository repository) => await repository.UpdateTicketDetailsContract(ticket, id) ? Results.NoContent() : Results.BadRequest()); 
        
        
        
        return group;
    }
    
}