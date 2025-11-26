using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace HabiTechs.Modules.Bookings.Models;

public enum BookingStatus
{
    Approved,
    Cancelled
}

public class Booking
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string ResidentId { get; set; }
    [ForeignKey("ResidentId")]
    public IdentityUser Resident { get; set; }
    
    [Required]
    public string AmenityName { get; set; }

    [Required]
    public DateOnly BookingDate { get; set; } 

    public BookingStatus Status { get; set; } = BookingStatus.Approved;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}