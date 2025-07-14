using System.ComponentModel.DataAnnotations;

namespace SupportSentral.Api.Contracts;

// List of all of our Contracts
public record UserContract(Guid Id, [Required]string Email, string Name);

public record UpdateUserContract(string Email, string Name);

public record CreateTicketContract(Guid Id, string Title, string Description, int StatusId, DateTime Created, string UserId);

public record UpdateTicketDetailsContract(string Title, string Description, int StatusId, DateTime Updated, string UserId);

public record StatusContract(int Id, string Name);



