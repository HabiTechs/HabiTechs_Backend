using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace HabiTechs.Modules.Finance.Models;

// Definición de métodos de pago
public enum PaymentMethod
{
    MercadoPago,
    Efectivo,
    TransferenciaQr, // Método para pagos locales (QR Simple)
    Otro
}

// Definición de estados del pago
public enum PaymentStatus
{
    Pending,  // Esperando que el Administrador revise el comprobante
    Approved, // Pago confirmado y deuda saldada
    Rejected  // Comprobante inválido o error
}

public class Payment
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string ResidentId { get; set; } = string.Empty;

    [ForeignKey("ResidentId")]
    public IdentityUser? Resident { get; set; }

    public Guid? ExpenseId { get; set; }
    [ForeignKey("ExpenseId")]
    public Expense? Expense { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

    public PaymentMethod Method { get; set; }
    
    // --- CAMPOS PARA AUDITORÍA Y FLUJO MANUAL ---
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending; // Estado del pago
    public string? ProofUrl { get; set; } // URL de la foto del comprobante subido a Azure
    // -------------------------------------------

    public string? ExternalReference { get; set; }
    public string? ValidatedBy { get; set; } // ID del Admin que aprobó
}