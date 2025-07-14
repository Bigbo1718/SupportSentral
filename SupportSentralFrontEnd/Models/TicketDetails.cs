using System.ComponentModel.DataAnnotations;

namespace SupportSentralFrontEnd.Models;

public class TicketDetails
{
    public int Id { get; set; }
    
    [Required] 
    [StringLength(50)]
    public required string Title { get; set; }
    [Required (ErrorMessage = "Please enter a valid error description")]
    public required string Description { get; set; }
    
    public string? StatusId { get; set; } 
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
    
    public string? UserId { get; set; }
    
    public bool AssignToSelf { get; set; }
}