using HabiTechs.Core.Data;
using HabiTechs.Modules.Access.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

// IMPORTANTE: Asegúrate de tener estos usings para los modelos
using HabiTechs.Modules.Users.Models;

namespace HabiTechs.Modules.Access.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Guardia,Admin")]
public class GuardController : ControllerBase
{
    private readonly AppDbContext _context;

    public GuardController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/Guard/metrics
    // Dashboard operacional (Entradas/Salidas/Paquetes)
    [HttpGet("metrics")]
    public async Task<ActionResult> GetGuardMetrics()
    {
        var today = DateTime.UtcNow.Date;

        // 1. Total de Entradas de Hoy (Usando GateLog)
        var entriesToday = await _context.GateLogs
            .CountAsync(g => g.AccessTime >= today && g.Direction == GateDirection.Entry);

        // 2. Total de Salidas de Hoy (Usando GateLog)
        var exitsToday = await _context.GateLogs
            .CountAsync(g => g.AccessTime >= today && g.Direction == GateDirection.Exit);

        // 3. Paquetes pendientes de recolección
        var pendingParcels = await _context.Parcels
            .CountAsync(p => p.PickedUpAt == null);
            
        // 4. Visitas Activas (Entraron, pero no han salido)
        var activeVisits = await _context.Visits
            .CountAsync(v => v.CheckedInAt != null && v.ExitedAt == null && v.IsFixedQRCode == false);

        return Ok(new 
        {
            EntriesToday = entriesToday,
            ExitsToday = exitsToday,
            PendingParcels = pendingParcels,
            ActiveVisits = activeVisits,
        });
    }

    // GET: api/Guard/recent-activity
    // Bitácora detallada (logs de la barrera)
    [HttpGet("recent-activity")]
    public async Task<ActionResult> GetRecentGateActivity()
    {
        var recentLogs = await _context.GateLogs
            .OrderByDescending(g => g.AccessTime)
            .Take(50)
            .Select(g => new 
            {
                g.AccessTime,
                Direction = g.Direction.ToString(),
                User = g.VisitorName ?? g.ResidentId,
                g.LicensePlate,
                Method = g.Method.ToString(),
            })
            .ToListAsync();
            
        return Ok(recentLogs);
    }
}