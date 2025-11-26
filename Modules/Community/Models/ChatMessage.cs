using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace HabiTechs.Modules.Community.Models;

public class ChatMessage
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string SenderId { get; set; } = string.Empty; // CORREGIDO

    [ForeignKey("SenderId")]
    public IdentityUser? Sender { get; set; } // Puede ser nulo al cargar

    [Required]
    public string ReceiverId { get; set; } = string.Empty; // CORREGIDO

    public string? Message { get; set; }
    public string? ImageUrl { get; set; }

    public DateTime SentAt { get; set; } = DateTime.UtcNow;

    [NotMapped]
    public bool IsExpired => SentAt < DateTime.UtcNow.AddDays(-30);
}