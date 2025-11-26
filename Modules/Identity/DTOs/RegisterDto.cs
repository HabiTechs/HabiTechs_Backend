using System.ComponentModel.DataAnnotations;

namespace HabiTechs.Modules.Identity.DTOs;

public class RegisterDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(6)]
    public string Password { get; set; }
    
    // Opcional: Pedir el rol en el registro
    // public string Role { get; set; } // Ej: "Residente" o "Admin"
}