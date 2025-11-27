namespace HabiTechs.Modules.Parcels.Models;


public enum ParcelStatus
{
    // Recibido por el guardia y en el almacén, esperando recogida.
    Received = 1, 
    // El residente ha validado el código y el paquete fue entregado.
    Delivered = 2, 
    // (Opcional) El paquete fue devuelto al remitente.
    Returned = 3 
}