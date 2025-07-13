namespace SupportSentral.Api.Contracts;

// List of all of our Contracts
public record UserContract(Guid Id, string Email, string Name);

public record CreateTicketContract(int Id, string Title, string Description, int StatusId, DateTime Created, string UserId);

public record UpdateTicketDetailsContract(int Id, string Title, string Description, int StatusId, DateTime Updated, string UserId);

public record StatusContract(int Id, string Name);



