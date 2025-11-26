using System.ComponentModel.DataAnnotations;

namespace HabiTechs.Modules.Access.DTOs;

public class CheckInDto
{
    [Required]
    public string QrCode { get; set; } = string.Empty; // Coincide con el modelo
}