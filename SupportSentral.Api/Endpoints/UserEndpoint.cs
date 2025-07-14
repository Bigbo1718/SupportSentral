using System.Text.RegularExpressions;
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

        const string getUserEndpointName = "GetUser";
        
        //Get /users
        group.MapGet("/", (SupportContext dbContext) =>(
            dbContext
                .Users
                .AsNoTracking()
                .ToList()) );
        
        //GET users/email
        group.MapGet("/{email}", (string email, SupportContext dbContext) =>
        {
            var user = dbContext.Users.SingleOrDefault(x => x.Email == email);
            
            return user != null ? 
                Results.Ok(user.MapToContract()) : Results.NotFound();
            
        }).WithName(getUserEndpointName);
        
        //Post users
        group.MapPost("/", (UserContract user, SupportContext dbContext) =>
        {
            if (string.IsNullOrWhiteSpace(user.Email))
            {
                return Results.BadRequest("Email is required");
            };

            var isValidEmail = user.Email.ValidateUsingRegex();

            if (!isValidEmail)
            {
                return Results.BadRequest("Please enter a valid email");
            }
            User createdUser = new User
            {
                Email = user.Email,
                Name = user.Name,
            };
            var createdEntity = dbContext.Users.Add(createdUser);
            dbContext.SaveChanges();
            var mappedUser= createdEntity.Entity.MapToContract();
            return Results.CreatedAtRoute(getUserEndpointName,
                new { mappedUser.Email}, mappedUser);
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
    
    public static bool ValidateUsingRegex(this string emailAddress)
    {
        var pattern = @"^[a-zA-Z0-9.!#$%&'*+-/=?^_`{|}~]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$";
            
        var regex = new Regex(pattern);

        return regex.IsMatch(emailAddress);
    }
}