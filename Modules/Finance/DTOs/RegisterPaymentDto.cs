using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http; // Necesario para IFormFile

namespace HabiTechs.Modules.Finance.DTOs;

public class RegisterPaymentDto
{
    [Required]
    public Guid ExpenseId { get; set; }
    
    // El comprobante subido por el residente (La foto del QR o transferencia)
    [Required]
    public IFormFile? ProofImage { get; set; } 
}