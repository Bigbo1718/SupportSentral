using Microsoft.EntityFrameworkCore;
using SupportSentral.Api.Contracts;
using SupportSentral.Api.Data;
using SupportSentral.Api.Entities;
using SupportSentral.Api.Mappings;

namespace SupportSentral.Api.Endpoints;

public static class UserEndpoint
{
    public static RouteGroupBuilder MapToUserEndpoint(this WebApplication app)
    {
        var group = app.MapGroup("users")
            .WithParameterValidation();

        const string getTicketEndpointName = "GetUser";
        
        //Get /users
        group.MapGet("/", (SupportContext dbContext) =>(
            dbContext
                .Users
                .AsNoTracking()
                .ToList()) );
        //Get /users
        group.MapGet("/{id}", (Guid id, SupportContext dbContext) =>
        {
            var user = dbContext.Users.Find(id);
            
            return user != null ? 
                Results.Ok(user.MapToContract()) : Results.NotFound();
            
        });
        //Post users
        group.MapPost("/", (UserContract user, SupportContext dbContext) =>
        {
            User createdUser = new User
            {
                Email = user.Email,
                Name = user.Name,
            };
            dbContext.Users.Add(createdUser);
            dbContext.SaveChanges();

        }); 
        //Post users
        group.MapPut("/{id}", (Guid id, UpdateUserContract user, SupportContext dbContext) =>
        {
            var existingUser = dbContext.Users.Find(id);
            if (existingUser == null)
            {
                Results.NotFound();
            }
            
            dbContext.Entry(existingUser)
                .CurrentValues
                .SetValues(user.MapToEntity(id));
            dbContext.SaveChanges();

        }); 
        
        return group;
    }
}