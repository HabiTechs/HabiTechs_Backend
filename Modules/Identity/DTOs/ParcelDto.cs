namespace HabiTechs.Modules.Access.DTOs;

// Este es el JSON que recibirá el Residente cuando consulte sus paquetes
public class ParcelDto
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public string ReceivedBy { get; set; }
    public DateTime ReceivedAt { get; set; }
    public bool IsPickedUp { get; set; } // True si ya fue recogido
}