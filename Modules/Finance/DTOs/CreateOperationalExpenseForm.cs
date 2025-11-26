using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http; // Necesario para IFormFile

namespace HabiTechs.Modules.Finance.DTOs;

public class CreateOperationalExpenseForm
{
    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public decimal Amount { get; set; }

    // La foto del recibo pagado (opcional)
    public IFormFile? ProofImage { get; set; }
}