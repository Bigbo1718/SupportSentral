using System.ComponentModel.DataAnnotations;

namespace SupportSentralFrontEnd.Models;

public class TicketDetails
{
    public Guid Id { get; set; }
    
    [Required] 
    [StringLength(50)]
    public required string Title { get; set; }
    [Required (ErrorMessage = "Please enter a valid error description")]
    public required string Description { get; set; }
    
    public int? StatusId { get; set; } 
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
    
    public Guid? UserId { get; set; }
    
    public bool AssignToSelf { get; set; }
}