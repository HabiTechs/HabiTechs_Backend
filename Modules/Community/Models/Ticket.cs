using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace HabiTechs.Modules.Community.Models;

public enum TicketStatus
{
    Open,
    InProgress,
    Closed
}

public class Ticket
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;
    
    // Foto del problema (subida por el residente al crear)
    public string? ImageUrl { get; set; } 

    public TicketStatus Status { get; set; } = TicketStatus.Open;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // --- CAMPOS PARA CIERRE / RESOLUCIÓN (Nuevos) ---
    public DateTime? ClosedAt { get; set; }
    
    // Comentario de cómo se solucionó (Ej: "Se cambió el foco")
    public string? ResolutionComment { get; set; } 
    
    // Foto de la prueba de solución (Ej: Foto del pasillo iluminado)
    public string? ResolutionImageUrl { get; set; } 
    // -----------------------------------------------

    [Required]
    public string RequesterId { get; set; } = string.Empty;
    
    [ForeignKey("RequesterId")]
    public IdentityUser? Requester { get; set; }
}