namespace HabiTechs.Modules.Finance.DTOs;

public class DashboardMetricsDto
{
    public decimal TotalIncomeThisMonth { get; set; }
    public int PendingExpensesCount { get; set; }
    public int MorososCount { get; set; } // Gente con deuda vencida
    public List<MonthlyIncomeDto> Last6MonthsIncome { get; set; } = new();
}

public class MonthlyIncomeDto
{
    public string Month { get; set; } = string.Empty; // Ej "Nov 2025"
    public decimal Amount { get; set; }
}