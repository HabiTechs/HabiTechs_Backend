using HabiTechs.Core.Data;
using HabiTechs.Modules.Access.DTOs;
using HabiTechs.Modules.Access.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HabiTechs.Modules.Access.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Residente,Admin,Guardia")]
public class GateController : ControllerBase
{
    private readonly AppDbContext _context;

    public GateController(AppDbContext context)
    {
        _context = context;
    }

    // POST: api/Gate/scan-fixed (Cuando el Residente escanea la pared)
    // Lógica para determinar si el movimiento es de ENTRADA o SALIDA
    [HttpPost("scan-fixed")]
    public async Task<IActionResult> ScanFixedQr(FixedQrScanDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();
        
        // 1. Verificar si el QR escaneado existe en el sistema como "Fijo"
        var validQr = await _context.Visits
            .FirstOrDefaultAsync(v => v.QRCode == dto.QrContent && v.IsFixedQRCode && v.IsApproved);

        if (validQr == null)
        {
            return BadRequest("Código QR no válido o inactivo.");
        }

        // 2. Obtener Placa y Dirección
        
        // Obtenemos la placa del perfil (asumiendo que el usuario ya actualizó su perfil)
        var profile = await _context.ResidentProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
        string? plate = profile?.LicensePlate;

        // Determinamos la dirección: Buscamos el último log de acceso de ESTE residente.
        var lastLog = await _context.GateLogs
            .Where(g => g.ResidentId == userId)
            .OrderByDescending(g => g.AccessTime)
            .FirstOrDefaultAsync();

        // Si no hay log previo, o la última acción fue una SALIDA, el nuevo movimiento es ENTRADA.
        // Si la última acción fue una ENTRADA, el nuevo movimiento es SALIDA.
        GateDirection direction = (lastLog == null || lastLog.Direction == GateDirection.Exit)
            ? GateDirection.Entry
            : GateDirection.Exit;

        // 3. Registrar en la bitácora (GateLog)
        var log = new GateLog
        {
            ResidentId = userId,
            LicensePlate = plate,
            Method = AccessMethod.FixedQrScanner,
            Direction = direction,
            AccessTime = DateTime.UtcNow
        };
        _context.GateLogs.Add(log);
        await _context.SaveChangesAsync();

        // 4. Retornar el estado
        string action = (direction == GateDirection.Entry) ? "ENTRADA REGISTRADA" : "SALIDA REGISTRADA";
        
        return Ok(new 
        { 
            message = $"Acceso Autorizado. {action}",
            direction = direction.ToString()
        });
    }
}