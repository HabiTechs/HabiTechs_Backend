using System.ComponentModel.DataAnnotations;

// (Namespace 100% Corregido)
namespace HabiTechs.Modules.Finance.DTOs;

// JSON que usa el Admin para cargar una nueva expensa
public class CreateExpenseDto
{
    [Required]
    [EmailAddress]
    public string ResidentEmail { get; set; }
    
    [Required]
    public string Title { get; set; }
    
    public string Description { get; set; }
    
    [Required]
    [Range(0.01, 1000000)]
    public decimal Amount { get; set; }
    
    [Required]
    public DateOnly DueDate { get; set; }
}