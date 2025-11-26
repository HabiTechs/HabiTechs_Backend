using System.ComponentModel.DataAnnotations;

namespace HabiTechs.Modules.Access.DTOs;

public class FixedQrScanDto
{
    [Required]
    public string QrContent { get; set; } = string.Empty; // El texto que tiene el QR fijo
}