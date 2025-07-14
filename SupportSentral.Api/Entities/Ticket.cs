using System.ComponentModel.DataAnnotations;

namespace SupportSentral.Api.Entities;

public class Ticket
{ 
    [Key]
    [Required]
    public Guid Id { get; set; }

    public required string Title { get; set; }
    
    public required string Description { get; set; }

    public required Guid UserId { get; set; }

    public required int StatusId { get; set; }

    public Status? Status { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }
    
}