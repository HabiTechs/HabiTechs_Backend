using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using HabiTechs.Modules.Users.Models; 

namespace HabiTechs.Modules.Access.Models;

public class Visit
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string QRCode { get; set; } = string.Empty;

    public bool IsFixedQRCode { get; set; } = false; 

    [Required]
    public string ResidentId { get; set; } = string.Empty;

    [ForeignKey("ResidentId")]
    public ResidentProfile? Resident { get; set; }

    public string VisitorName { get; set; } = string.Empty; 
    
    public string? Notes { get; set; } 

    public bool IsApproved { get; set; } = false; 

    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? ApprovedAt { get; set; }

    // --- CICLO DE ACCESO ---
    public DateTime? CheckedInAt { get; set; } // HORA DE ENTRADA
    public DateTime? ExitedAt { get; set; }    // NUEVO: HORA DE SALIDA

    public string? ApprovedByGuardId { get; set; } 
    
    [ForeignKey("ApprovedByGuardId")]
    public IdentityUser? Guard { get; set; }

    public bool IsEntryCompleted { get; set; } = false; 
}