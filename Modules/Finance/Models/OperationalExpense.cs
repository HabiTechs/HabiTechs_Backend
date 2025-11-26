using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabiTechs.Modules.Finance.Models;

// Representa un gasto incurrido por el condominio (pago de luz, agua, salario)
public class OperationalExpense
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(150)]
    public string Title { get; set; } = string.Empty; // Ej: Factura de Electricidad, Salario Guardia

    public string? Description { get; set; }
    
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    public DateTime DateIncurred { get; set; } = DateTime.UtcNow;

    // Foto del recibo/factura pagada (Evidencia)
    public string? ProofImageUrl { get; set; } 
}