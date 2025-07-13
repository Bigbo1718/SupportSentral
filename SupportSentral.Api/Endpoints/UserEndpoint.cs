using Microsoft.EntityFrameworkCore;
using SupportSentral.Api.Contracts;
using SupportSentral.Api.Data;
using SupportSentral.Api.Entities;

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
        
        return group;
    }
}