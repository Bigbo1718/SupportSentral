using System.ComponentModel.DataAnnotations;

namespace SupportSentral.Api.Contracts;

// List of all of our Contracts
public record UserContract(Guid Id, [Required] string Email, string Name);

public record UpdateUserContract(string Email, string Name);

public record CreateTicketContract([Required] string Title, [Required] string Description,[Required] int StatusId, DateTime Created, [Required]Guid UserId);

public record TicketDetailsContract(Guid Id, [Required] string Title, [Required] string Description, int StatusId, DateTime Created, DateTime? UpdatedDateTime,[Required] Guid UserId);

public record TicketContract(Guid Id, [Required] string Title, [Required] string Description, string Status, DateTime CreatedDateTime, DateTime? UpdatedDateTime,Guid UserId);


public record UpdateTicketDetailsContract([Required] string Title, [Required] string Description, int StatusId, DateTime Updated, Guid UserId);

public record StatusContract(int Id, string Name);



