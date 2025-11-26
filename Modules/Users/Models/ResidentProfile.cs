using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace HabiTechs.Modules.Users.Models;

public enum AccountType
{
    Principal,
    Secundaria
}

public class ResidentProfile
{
    [Key]
    public string UserId { get; set; } = string.Empty; 

    [ForeignKey("UserId")]
    public IdentityUser? User { get; set; }

    [Required]
    public string FullName { get; set; } = string.Empty; 

    [Required]
    [StringLength(6)]
    public string ResidentCode { get; set; } = string.Empty; 

    public string? PhotoUrl { get; set; }
    
    // --- CAMPOS EXISTENTES ---
    public string? PhoneNumber { get; set; } // Celular Principal
    
    // --- NUEVOS CAMPOS (AGREGADOS PARA LA VERSIÓN FINAL) ---
    public string? IdentityCard { get; set; }   // Cédula de Identidad
    public string? Occupation { get; set; }     // Ocupación
    public string? SecondaryPhone { get; set; } // Celular de Respaldo
    public string? LicensePlate { get; set; }   // Placa de Vehículo (Para Barrera)
    // -------------------------------------------------------

    public AccountType AccountType { get; set; } = AccountType.Principal;
    public bool IsActive { get; set; } = true;
    public string? ParentProfileId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}