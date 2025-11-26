using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace HabiTechs.Modules.Users.DTOs;

public class UserProfileForm
{
    [Required] 
    public string FullName { get; set; } = string.Empty;
    
    public string? IdentityCard { get; set; }
    public string? Occupation { get; set; }
    public string? PhoneNumber { get; set; }
    public string? SecondaryPhone { get; set; }
    public string? LicensePlate { get; set; } // ¡La Placa!

    public IFormFile? Photo { get; set; }
}