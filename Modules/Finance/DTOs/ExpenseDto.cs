// (Namespace 100% Corregido)
namespace HabiTechs.Modules.Finance.DTOs;

// JSON que recibe el Residente/Admin al consultar
public class ExpenseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsPaid { get; set; }
    public DateTime? PaidAt { get; set; }
    public string ResidentEmail { get; set; }
}