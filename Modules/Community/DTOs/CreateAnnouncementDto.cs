using System.ComponentModel.DataAnnotations;

namespace HabiTechs.Modules.Community.DTOs;

// Este es el JSON que enviará el Admin para crear un anuncio
public class CreateAnnouncementDto
{
    [Required]
    [MaxLength(100)]
    public string Title { get; set; }

    [Required]
    public string Content { get; set; }
}