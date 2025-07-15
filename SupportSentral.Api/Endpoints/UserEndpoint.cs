using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using SupportSentral.Api.Contracts;
using SupportSentral.Api.Data;
using SupportSentral.Api.Entities;
using SupportSentral.Api.Mappings;
using SupportSentral.Api.Repositories;

namespace SupportSentral.Api.Endpoints;

public static class UserEndpoint
{
    public static RouteGroupBuilder MapToUserEndpoint(this WebApplication app)
    {
        var group = app.MapGroup("users")
            .WithParameterValidation();

        const string getUserEndpointName = "GetUser";
        
        //Get /users
        group.MapGet("/", (IUserRepository repository) =>(
            repository.GetAllAsync()) );
        
        //GET users/email
        group.MapGet("/id/{id}",async (Guid Id, 
            IUserRepository repository)  =>
        {
            var user = await repository.GetByIdAsync(Id);
            
            return user != null ? 
                Results.Ok(user) : Results.NotFound();
            
        }).WithName(getUserEndpointName);
        
        //GET users/email
        group.MapGet("/{email}",async (string email, 
            IUserRepository repository)  =>
        {
            var user = await repository.GetByEmailIdAsync(email);
            
            return user != null ? 
                Results.Ok(user) : Results.NotFound();
            
        }).WithName(getUserEndpointName);
        
        //POST users
        group.MapPost("/", async (UserContract user, IUserRepository repository) =>
        {
            UserContract? createdUser = await repository.CreateUserAsync(user);
            
            if(createdUser == null)
                return Results.BadRequest();
            
            return Results.CreatedAtRoute(getUserEndpointName,
                new { createdUser.Email}, createdUser);
        }); 
        //PUT users
        group.MapPut("/{id}", async (Guid id, UpdateUserContract user,IUserRepository repository) => await repository.UpdateUser(id, user)?  Results.NoContent(): Results.BadRequest()); 
        
        
        return group;
    }
    
    public static bool ValidateUsingRegex(this string emailAddress)
    {
        var pattern = @"^[a-zA-Z0-9.!#$%&'*+-/=?^_`{|}~]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$";
            
        var regex = new Regex(pattern);

        return regex.IsMatch(emailAddress);
    }
}