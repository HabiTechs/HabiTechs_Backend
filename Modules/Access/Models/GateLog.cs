using System.ComponentModel.DataAnnotations;

namespace HabiTechs.Modules.Access.Models;

// Nuevo Enum para saber si entra o sale
public enum GateDirection
{
    Entry, // Entrada
    Exit   // Salida
}

public enum AccessMethod
{
    FixedQrScanner,
    GuardManual,
    VisitQr
}

public class GateLog
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public string? ResidentId { get; set; }
    
    public string? VisitorName { get; set; }
    
    // NUEVO: Placa del vehículo (si aplica)
    public string? LicensePlate { get; set; } 

    public DateTime AccessTime { get; set; } = DateTime.UtcNow;
    
    public AccessMethod Method { get; set; }
    
    // NUEVO: Dirección del movimiento
    public GateDirection Direction { get; set; } = GateDirection.Entry; 
    
    public string? GuardId { get; set; }
}