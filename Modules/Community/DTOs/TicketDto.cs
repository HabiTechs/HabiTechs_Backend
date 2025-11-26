namespace HabiTechs.Modules.Community.DTOs;

// JSON que recibe el Residente/Admin al leer los tickets
public class TicketDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Status { get; set; } // Lo convertimos a string para que sea fácil de leer
    public DateTime CreatedAt { get; set; }
    public DateTime? ClosedAt { get; set; }
    public string RequesterEmail { get; set; }
}