using System.Security.Claims;
using HabiTechs.Core.Data;
using HabiTechs.Modules.Access.DTOs;
using HabiTechs.Modules.Access.Hubs;
using HabiTechs.Modules.Access.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

// CORREGIDO: El namespace coincide con tu carpeta "Identity/Controllers"
namespace HabiTechs.Modules.Identity.Controllers; 

[ApiController]
[Route("api/[controller]")]
[Authorize] 
public class ParcelController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IHubContext<AccessHub> _hubContext;

    public ParcelController(AppDbContext context, UserManager<IdentityUser> userManager, IHubContext<AccessHub> hubContext)
    {
        _context = context;
        _userManager = userManager;
        _hubContext = hubContext;
    }
    
    // 1. GET: RESIDENTE ve sus paquetes
    [HttpGet("my-parcels")]
    [Authorize(Roles = "Residente")]
    public async Task<ActionResult<IEnumerable<object>>> GetMyParcels()
    {
        var residentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var parcels = await _context.Parcels
            .Where(p => p.ResidentId == residentId)
            .OrderByDescending(p => p.ReceivedAt)
            .Select(p => new 
            {
                p.Id,
                p.Description,
                p.ReceivedBy,
                p.ReceivedAt,
                IsPickedUp = p.PickedUpAt != null
            })
            .ToListAsync();
        return Ok(parcels);
    }
    
    // 2. POST: GUARDIA registra un paquete (ENTRADA)
    [HttpPost("register")]
    [Authorize(Roles = "Guardia")]
    public async Task<IActionResult> RegisterParcel(CreateParcelDto createParcelDto)
    {
        var resident = await _userManager.FindByEmailAsync(createParcelDto.ResidentEmail);
        if (resident == null)
        {
            return BadRequest("No se encontró un residente con ese email.");
        }
        
        var guardEmail = User.FindFirstValue(ClaimTypes.Email);

        var parcel = new Parcel
        {
            ResidentId = resident.Id,
            Description = createParcelDto.Description,
            ReceivedBy = guardEmail ?? "Guardia", // Guardia de turno que recibió
            ReceivedAt = DateTime.UtcNow
        };

        await _context.Parcels.AddAsync(parcel);
        await _context.SaveChangesAsync();
        
        // Notificación en tiempo real al residente
        await _hubContext.Clients.User(resident.Id).SendAsync(
            "ParcelReceived", 
            $"Tienes un nuevo paquete: {parcel.Description}"
        );

        return CreatedAtAction(nameof(GetMyParcels), new { id = parcel.Id }, parcel);
    }
    
    // 3. POST: RESIDENTE escanea el paquete para confirmar recogida (SALIDA/HANDOVER)
    [HttpPost("{parcelId}/pickup")]
    [Authorize(Roles = "Residente,Guardia")] // El Guardia lo dispara, pero se registra quién lo recibió
    public async Task<IActionResult> PickUpParcel(Guid parcelId)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var parcel = await _context.Parcels.FindAsync(parcelId);

        if (parcel == null)
        {
            return NotFound("Paquete no encontrado.");
        }
        
        if (parcel.PickedUpAt != null)
        {
            return BadRequest("Este paquete ya fue recogido.");
        }
        
        // El Guardia DEBE verificar que el ID que escanea sea el del residente asignado.
        // Aquí simplificamos, asumiendo que el residente presenta su QR/ID.
        // Lógica de validación de propiedad (el residente logueado debe ser el dueño)
        if (parcel.ResidentId != currentUserId && !User.IsInRole("Guardia")) 
        {
             return Forbid("No tienes permiso para marcar este paquete como recogido.");
        }

        // Marcar como recogido por el residente
        parcel.PickedUpAt = DateTime.UtcNow;
        _context.Parcels.Update(parcel);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Paquete entregado y recogido exitosamente." });
    }
}