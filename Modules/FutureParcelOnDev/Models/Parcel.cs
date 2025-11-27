using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace HabiTechs.Modules.Parcels.Models;

/// <summary>
/// Representa un paquete recibido en el condominio para un residente.
/// </summary>
public class Parcel
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    // ID del residente destinatario
    [Required]
    public string ResidentId { get; set; } = string.Empty;
    [ForeignKey("ResidentId")]
    public IdentityUser? Resident { get; set; }

    // Campos de descripción
    public string Description { get; set; } = string.Empty;
    
    // Campos del flujo de recepción
    public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
    public string ReceivedByGuardId { get; set; } = string.Empty; // ID del guardia que lo recibió
    
    // --- CAMPOS CLAVE PARA EL NUEVO FLUJO ---
    
    /// <summary>
    /// Código alfanumérico corto y único que el residente debe presentar para recoger el paquete (Ej: 25A7).
    /// </summary>
    [Required]
    [MaxLength(10)]
    public string PickupCode { get; set; } = string.Empty;
    
    /// <summary>
    /// Ubicación física en el almacén (Ej: Estantería C-12, Locker 5).
    /// </summary>
    public string? StorageLocation { get; set; } 
    
    /// <summary>
    /// Cadena de datos para generar el QR impreso en el paquete (Ej: "HABITECHS|ID:GUID|CODE:25A7").
    /// </summary>
    public string QrCodeData { get; set; } = string.Empty;

    // --- CAMPOS DE ESTADO Y ENTREGA ---

    public ParcelStatus Status { get; set; } = ParcelStatus.Received;
    public DateTime? DeliveredAt { get; set; }
    public string? DeliveredByGuardId { get; set; } // ID del guardia que entregó
}