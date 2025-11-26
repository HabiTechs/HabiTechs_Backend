using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace HabiTechs.Modules.Access.Hubs;

// [Authorize] // Asegura que solo usuarios logueados se conecten al Hub
public class AccessHub : Hub
{
    // El frontend de Residente (Dev 2) se conectará a esto
    public async Task SendVisitArrival(string userId, string visitorName)
    {
        // Notifica a un residente específico por su UserID
        await Clients.User(userId).SendAsync("VisitArrived", visitorName);
    }

    public async Task SendParcelArrival(string userId, string parcelType)
    {
        await Clients.User(userId).SendAsync("ParcelArrived", parcelType);
    }

    // Cuando un usuario se conecta, podemos obtener su ID
    public override async Task OnConnectedAsync()
    {
        // var userId = Context.UserIdentifier; // Obtiene el ID del usuario del token JWT
        // await Groups.AddToGroupAsync(Context.ConnectionId, $"condo_{userId}");
        await base.OnConnectedAsync();
    }
}