using System.ComponentModel.DataAnnotations;

namespace HabiTechs.Modules.Bookings.Models;

public class CommonArea
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty; // Inicializado para evitar error

    public string? Description { get; set; } 

    public bool IsActive { get; set; } = true; 

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}