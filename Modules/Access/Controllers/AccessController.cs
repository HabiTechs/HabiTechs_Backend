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

namespace HabiTechs.Modules.Access.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AccessController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IHubContext<AccessHub> _hubContext;

    public AccessController(AppDbContext context, UserManager<IdentityUser> userManager, IHubContext<AccessHub> hubContext)
    {
        _context = context;
        _userManager = userManager;
        _hubContext = hubContext;
    }

    // --- GENERAR QR VISITA (Residente) ---
    [HttpPost("visit/generate-qr")]
    [Authorize(Roles = "Residente")]
    public async Task<IActionResult> GenerateVisitQr(CreateVisitDto createVisitDto)
    {
        var residentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (residentId == null) return Unauthorized("Usuario no encontrado.");

        var visit = new Visit
        {
            ResidentId = residentId,
            VisitorName = createVisitDto.VisitorName,
            Notes = createVisitDto.Notes,
            QRCode = Guid.NewGuid().ToString("N"),
            IsApproved = false, 
            RequestedAt = DateTime.UtcNow,
            IsFixedQRCode = false
        };

        await _context.Visits.AddAsync(visit);
        await _context.SaveChangesAsync();

        return Ok(new { qrCode = visit.QRCode });
    }

    // --- GENERAR QR FIJO (Admin/Residente) ---
    [HttpPost("visit/generate-fixed-qr")]
    [Authorize(Roles = "Residente,Admin")]
    public async Task<IActionResult> GenerateFixedQr()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var visit = new Visit
        {
            ResidentId = userId!,
            VisitorName = "QR Fijo Residente",
            QRCode = Guid.NewGuid().ToString("N"),
            IsApproved = true, 
            RequestedAt = DateTime.UtcNow,
            IsFixedQRCode = true,
            IsEntryCompleted = false 
        };

        await _context.Visits.AddAsync(visit);
        await _context.SaveChangesAsync();

        return Ok(new { qrCode = visit.QRCode });
    }

    // --- CHECK-IN / CHECK-OUT (Guardia) ---
    [HttpPost("visit/check-in")]
    [Authorize(Roles = "Guardia")]
    public async Task<IActionResult> CheckInVisit(CheckInDto checkInDto)
    {
        var visit = await _context.Visits
            .FirstOrDefaultAsync(v => v.QRCode == checkInDto.QrCode);

        if (visit == null) return BadRequest("QR inválido.");

        // 1. QR FIJO (Residente) - Solo valida, no quema
        if (visit.IsFixedQRCode)
        {
            if (!visit.IsApproved) return BadRequest("QR fijo inhabilitado.");
            
            // Registrar log de acceso
            var log = new GateLog 
            { 
                ResidentId = visit.ResidentId, 
                Method = AccessMethod.FixedQrScanner,
                Direction = GateDirection.Entry, 
                GuardId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            };
            _context.GateLogs.Add(log);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Residente - Acceso Autorizado" });
        }

        // 2. QR VISITA (Entrada/Salida)

        // A. Si ya salió, el QR murió
        if (visit.ExitedAt != null)
        {
             return BadRequest("Este pase ya fue cerrado (Visita finalizada).");
        }

        // B. Si ya entró pero no salió -> REGISTRAR SALIDA
        if (visit.CheckedInAt != null && visit.ExitedAt == null)
        {
            visit.ExitedAt = DateTime.UtcNow;
            _context.Visits.Update(visit);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"SALIDA registrada para: {visit.VisitorName}" });
        }

        // C. Si no ha entrado -> REGISTRAR ENTRADA
        if (visit.CheckedInAt == null)
        {
            visit.IsApproved = true; // Auto-aprobar al escanear
            visit.ApprovedByGuardId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            visit.ApprovedAt = DateTime.UtcNow;
            visit.CheckedInAt = DateTime.UtcNow;
            visit.IsEntryCompleted = true; // Legacy flag

            _context.Visits.Update(visit);
            await _context.SaveChangesAsync();

            // Notificar al residente
            await _hubContext.Clients.User(visit.ResidentId).SendAsync(
                "VisitArrived",
                $"Tu visita '{visit.VisitorName}' acaba de INGRESAR."
            );

            return Ok(new { message = $"ENTRADA registrada para: {visit.VisitorName}" });
        }

        return BadRequest("Estado del QR desconocido.");
    }

    // --- REGISTRAR VISITA MANUAL (Guardia) ---
    [HttpPost("visit/manual-register")]
    [Authorize(Roles = "Guardia")]
    public async Task<IActionResult> RegisterVisitManually(ManualVisitDto dto)
    {
        var resident = await _userManager.FindByIdAsync(dto.ResidentId);
        if (resident == null) return BadRequest("Residente no encontrado.");

        var visit = new Visit
        {
            ResidentId = dto.ResidentId,
            VisitorName = dto.VisitorName,
            Notes = dto.Notes,
            QRCode = "MANUAL-" + Guid.NewGuid().ToString("N").Substring(0,8),
            IsApproved = true, 
            RequestedAt = DateTime.UtcNow,
            ApprovedAt = DateTime.UtcNow,
            CheckedInAt = DateTime.UtcNow, // Entra directo
            IsEntryCompleted = true,
            IsFixedQRCode = false,
            ApprovedByGuardId = User.FindFirstValue(ClaimTypes.NameIdentifier)
        };

        await _context.Visits.AddAsync(visit);
        await _context.SaveChangesAsync();

        await _hubContext.Clients.User(dto.ResidentId).SendAsync(
            "VisitArrived",
            $"Visita '{dto.VisitorName}' registrada manualmente por guardia."
        );

        return Ok(new { message = $"Ingreso Manual: {dto.VisitorName}" });
    }
}