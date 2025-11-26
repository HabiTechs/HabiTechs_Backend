using System.ComponentModel.DataAnnotations;

namespace HabiTechs.Modules.Bookings.DTOs;

public class CreateBookingDto
{
    [Required]
    public string AmenityName { get; set; } 

    [Required]
    public DateOnly BookingDate { get; set; }
}