namespace SupportSentralFrontEnd.Models;

public class Ticket
{
    public Guid Id { get; set; }
    
    public required string Title { get; set; }
    
    public required string Description { get; set; }
    
    public int? StatusId { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
    
    public required Guid UserId { get; set; }

    public string? AssignedTo { get; set; }
}
