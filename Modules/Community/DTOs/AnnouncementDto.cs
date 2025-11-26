namespace HabiTechs.Modules.Community.DTOs;

// Este es el JSON que recibirá el Residente/Guardia al leer los anuncios
public class AnnouncementDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public string AuthorEmail { get; set; } // Para saber qué admin lo publicó
}