using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace HabiTechs.Modules.Access.Models;

public class Parcel
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string ResidentId { get; set; }
    [ForeignKey("ResidentId")]
    public IdentityUser Resident { get; set; }
    
    public string Description { get; set; }
    
    // Esta es la propiedad que te faltaba y causaba el error rojo
    public string ReceivedBy { get; set; } 

    public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
    public DateTime? NotifiedAt { get; set; }
    public DateTime? PickedUpAt { get; set; }
}