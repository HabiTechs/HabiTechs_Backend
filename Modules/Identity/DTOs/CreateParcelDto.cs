using System.ComponentModel.DataAnnotations;

namespace HabiTechs.Modules.Access.DTOs;

// Este es el JSON que enviará el Guardia al registrar un paquete
public class CreateParcelDto
{
    [Required]
    [EmailAddress]
    public string ResidentEmail { get; set; }

    [Required]
    public string Description { get; set; }
}