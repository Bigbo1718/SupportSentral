using System.ComponentModel.DataAnnotations;

namespace SupportSentral.Api.Entities;

public class User
{
    [Key]
    [Required]
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
}