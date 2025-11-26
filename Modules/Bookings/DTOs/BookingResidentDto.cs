namespace HabiTechs.Modules.Bookings.DTOs;

public class BookingResidentDto
{
    public Guid Id { get; set; }
    public string AmenityName { get; set; } = string.Empty;
    public DateOnly BookingDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string? ResidentCode { get; set; }
}