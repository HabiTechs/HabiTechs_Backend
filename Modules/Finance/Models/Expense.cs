using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

// (Namespace 100% Corregido)
namespace HabiTechs.Modules.Finance.Models;

// Representa una sola línea de cargo (ej. "Expensa Noviembre 2025")
public class Expense
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    // A quién se le cobra
    [Required]
    public string ResidentId { get; set; }
    [ForeignKey("ResidentId")]
    public IdentityUser Resident { get; set; }

    [Required]
    [MaxLength(100)]
    public string Title { get; set; } // "Expensa Noviembre 2025"

    public string Description { get; set; } // "Alícuota, fondo de reserva, etc."

    [Required]
    public decimal Amount { get; set; } // El monto
    
    public DateTime DueDate { get; set; } // Fecha de vencimiento
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public bool IsPaid { get; set; } = false;
    public DateTime? PaidAt { get; set; }
}