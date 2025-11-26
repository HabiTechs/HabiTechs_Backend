using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace HabiTechs.Modules.Community.DTOs;

public class CloseTicketForm
{
    [Required]
    public string Comment { get; set; } = string.Empty; // Comentario obligatorio al cerrar

    public IFormFile? EvidenceImage { get; set; } // Foto opcional de "trabajo terminado"
}