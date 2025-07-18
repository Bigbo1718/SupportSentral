using System.ComponentModel.DataAnnotations;

namespace SupportSentralFrontEnd.Models;

public class User
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    [Required]
    public required string Email { get; set; }
}