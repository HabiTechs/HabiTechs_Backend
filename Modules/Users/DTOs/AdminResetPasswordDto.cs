using System.ComponentModel.DataAnnotations;

namespace HabiTechs.Modules.Users.DTOs;

public class AdminResetPasswordDto
{
    [Required]
    [EmailAddress]
    public string TargetEmail { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string NewPassword { get; set; } = string.Empty;
}