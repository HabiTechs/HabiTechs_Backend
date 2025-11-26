using System.ComponentModel.DataAnnotations;

namespace HabiTechs.Modules.Community.DTOs;

public class SendMessageDto
{
    [Required]
    public string ReceiverId { get; set; } = string.Empty; // ID del Admin o Residente destino
    
    public string Message { get; set; } = string.Empty;
    
    // La imagen se maneja vía IFormFile en el controlador, aquí es opcional
}