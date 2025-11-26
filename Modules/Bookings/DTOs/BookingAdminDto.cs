namespace HabiTechs.Modules.Bookings.DTOs;

public class BookingAdminDto
{
    public Guid Id { get; set; }
    public string AmenityName { get; set; } = string.Empty;
    public DateOnly BookingDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    // Pueden ser nulos si el usuario se borró, así que usamos ?
    public string? ResidentEmail { get; set; }
    public string? ResidentName { get; set; }
    public string? ResidentCode { get; set; }
}