using System.ComponentModel.DataAnnotations;

namespace HabiTechs.Modules.Access.DTOs;

public class CreateVisitDto
{
    [Required]
    public string VisitorName { get; set; } = string.Empty;

    // ESTA ES LA PROPIEDAD QUE FALTABA
    public string? Notes { get; set; } 
}