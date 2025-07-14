using Microsoft.EntityFrameworkCore;
using SupportSentral.Api.Contracts;
using SupportSentral.Api.Data;
using SupportSentral.Api.Mappings;

namespace SupportSentral.Api.Endpoints;

public static class TicketEndpoints 
{
     public static RouteGroupBuilder MapToTicketEndpoint(this WebApplication app)
    {
        var group = app.MapGroup("tickets")
            .WithParameterValidation();

        const string getTicketEndpointName = "GetTicket";
        
        //Get /tickets
        group.MapGet("/", (SupportContext dbContext) =>(
            dbContext
                .Tickets
                .AsNoTracking()
                .ToList()) );
        
        //GET tickets/id
        group.MapGet("/{id}", (Guid id, SupportContext dbContext) =>
        {
            var ticket = dbContext.Tickets.Find(id);
            
            return ticket != null ? 
                Results.Ok(ticket.ToTicketDetailsContract()) : Results.NotFound();
            
        }).WithName(getTicketEndpointName);
        
        //Post tickets/
        group.MapPost("/", (CreateTicketContract ticket, SupportContext dbContext) =>
        {
            if (ticket.UserId.Equals(Guid.Empty))
            {
                return Results.BadRequest("a valid user id is required");
            };

            if (ticket.StatusId.Equals(0))
            {
                return Results.BadRequest("a valid status id is required");
            }
            
            var createdTicketEntity = dbContext.Tickets.Add(ticket.ToEntity());
            dbContext.SaveChanges();
            var ticketDetails = createdTicketEntity.Entity.ToTicketDetailsContract();
            return Results.CreatedAtRoute(getTicketEndpointName,
                new { ticketDetails.Id}, ticketDetails);
        }).WithParameterValidation();; 
        
        //Put tickets
        group.MapPut("/{id}", (Guid id, UpdateTicketDetailsContract ticket, SupportContext dbContext) =>
        {
            var existingTicket = dbContext.Tickets.Find(id);
            if (existingTicket == null)
            {
               return Results.NotFound();
            }
            if (ticket.StatusId.Equals(0))
            {
                return Results.BadRequest("a valid status id is required");
            }
            
            dbContext.Entry(existingTicket)
                .CurrentValues
                .SetValues(ticket.ToEntity(id));
            dbContext.SaveChanges();

            return Results.NoContent();
        }); 
        
        
        
        return group;
    }
    
}