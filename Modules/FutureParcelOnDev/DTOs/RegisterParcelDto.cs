using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace HabiTechs.Modules.Parcels.DTOs;


public class RegisterParcelDto
{
    [Required]
    [EmailAddress]
    [Display(Name = "Email del Residente")]
    public string ResidentEmail { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Descripción o Empresa de Envío")]
    public string Description { get; set; } = string.Empty;
    
    [Display(Name = "Ubicación en Almacén")]
    public string? StorageLocation { get; set; } 

    [Required]
    [Display(Name = "Foto del Paquete/Guía")]
  
    public IFormFile Image { get; set; } = null!; 
}