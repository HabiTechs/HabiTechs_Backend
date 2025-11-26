using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace HabiTechs.Modules.Community.DTOs;

public class CreateAnnouncementForm
{
    [Required] public string Title { get; set; } = string.Empty;
    [Required] public string Content { get; set; } = string.Empty;
    public IFormFile? Image { get; set; }
}

public class CreateTicketForm
{
    [Required] public string Title { get; set; } = string.Empty;
    [Required] public string Description { get; set; } = string.Empty;
    public IFormFile? Image { get; set; }
}

public class SendMessageForm
{
    [Required] public string ReceiverId { get; set; } = string.Empty;
    public string? Message { get; set; }
    public IFormFile? Image { get; set; }
}