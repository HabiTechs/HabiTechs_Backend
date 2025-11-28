namespace HabiTechs.Modules.FutureParcelOnDev.DTOs;


public class ParcelDetailsDto
{
    public Guid Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string ReceivedByGuardEmail { get; set; } = string.Empty;
    public DateTime ReceivedAt { get; set; }
    public DateTime? DeliveredAt { get; set; }
    

    public string PickupCode { get; set; } = string.Empty; 
    

    public string? StorageLocation { get; set; }
    

    public string ImageUrl { get; set; } = string.Empty;
    

    public string QrCodeData { get; set; } = string.Empty; 
}