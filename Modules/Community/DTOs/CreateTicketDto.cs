using System.ComponentModel.DataAnnotations;

namespace HabiTechs.Modules.Community.DTOs;

// JSON que envía el Residente al crear un ticket
public class CreateTicketDto
{
    [Required]
    [MaxLength(100)]
    public string Title { get; set; }

    [Required]
    public string Description { get; set; }
}