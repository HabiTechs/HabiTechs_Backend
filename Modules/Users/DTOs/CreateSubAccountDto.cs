using System.ComponentModel.DataAnnotations;

namespace HabiTechs.Modules.Users.DTOs;

public class CreateSubAccountDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string FullName { get; set; } = string.Empty;
    
    [Required]
    public string Password { get; set; } = string.Empty;
}