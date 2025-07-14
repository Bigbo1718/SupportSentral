using Microsoft.EntityFrameworkCore;
using SupportSentral.Api.Contracts;
using SupportSentral.Api.Data;
using SupportSentral.Api.Repositories;

namespace SupportSentral.Api.Endpoints;

public static class StatusEndpoints
{
    public static RouteGroupBuilder MapToStatusEndpoint(this WebApplication app)
    {
        var group = app.MapGroup("status")
            .WithParameterValidation();

        const string getUserEndpointName = "GetStatus";
        
        //Get /status
        group.MapGet("/", async (SupportContext dbContext)=> await dbContext.Status.AsNoTracking().ToListAsync());
        
        //GET status/id
        group.MapGet("/{id}",async (int id,  SupportContext dbContext)  =>
        {
            var status = await dbContext.Status.FindAsync(id);
            if (status != null) 
                return Results.BadRequest();

            return Results.Ok(status);
        }).WithName(getUserEndpointName);
        
        return group;
    }

}