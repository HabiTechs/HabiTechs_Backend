using System.ComponentModel.DataAnnotations;

namespace HabiTechs.Modules.Access.DTOs;

public class ManualVisitDto
{
    [Required]
    public string ResidentId { get; set; } = string.Empty;

    [Required]
    public string VisitorName { get; set; } = string.Empty;

    public string? Notes { get; set; }
}