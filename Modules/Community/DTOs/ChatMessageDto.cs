namespace HabiTechs.Modules.Community.DTOs;

public class ChatMessageDto
{
    public Guid Id { get; set; }
    public string SenderName { get; set; } = string.Empty; // Nombre de quien envía
    public string SenderId { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public DateTime SentAt { get; set; }
    public bool IsMine { get; set; } // True si yo lo envié
}