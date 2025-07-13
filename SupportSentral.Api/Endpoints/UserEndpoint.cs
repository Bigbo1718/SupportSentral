namespace SupportSentral.Api.Endpoints;

public static class UserEndpoint
{
    public static RouteGroupBuilder MapToUserEndpoint(this WebApplication app)
    {
        var group = app.MapGroup("users")
            .WithParameterValidation();

        const string getTicketEndpointName = "GetTicket";

        group.MapGet("/", () => "Helloooo You Made It Here User");
        
        return group;
    }
}