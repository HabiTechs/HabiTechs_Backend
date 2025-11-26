using System.ComponentModel.DataAnnotations;

namespace HabiTechs.Modules.Finance.Models;

public class PaymentInstruction
{
    [Key]
    public int Id { get; set; } // Usamos 1 como clave para asegurar una sola entrada

    [Required] // AÑADIDO: No puede estar vacío
    public string BankName { get; set; } = string.Empty;
    
    [Required] // AÑADIDO: No puede estar vacío
    public string AccountNumber { get; set; } = string.Empty;
    
    [Required] // AÑADIDO: No puede estar vacío
    public string AccountHolder { get; set; } = string.Empty;
    
    [Required] // AÑADIDO: No puede estar vacío
    public string NitroId { get; set; } = string.Empty; // CI o NIT
    
    public string? QrImageUrl { get; set; } // Foto del QR Fijo (Opcional)
}