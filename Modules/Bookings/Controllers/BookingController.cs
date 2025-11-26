using HabiTechs.Core.Data;
using HabiTechs.Modules.Bookings.DTOs;
using HabiTechs.Modules.Bookings.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace HabiTechs.Modules.Bookings.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BookingController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public BookingController(AppDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // --- RESIDENTE ---
    [HttpGet("my-bookings")]
    [Authorize(Roles = "Residente")]
    public async Task<ActionResult<IEnumerable<BookingResidentDto>>> GetMyBookings()
    {
        var residentId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var bookings = await _context.Bookings
            .Include(b => b.Resident)
            .Where(b => b.ResidentId == residentId && b.Status == BookingStatus.Approved)
            .OrderBy(b => b.BookingDate)
            .Select(b => new BookingResidentDto
            {
                Id = b.Id,
                AmenityName = b.AmenityName,
                BookingDate = b.BookingDate,
                Status = b.Status.ToString(),
                CreatedAt = b.CreatedAt,
                // CORREGIDO: Usamos ResidentCode
                ResidentCode = _context.ResidentProfiles
                                    .Where(p => p.UserId == b.ResidentId)
                                    .Select(p => p.ResidentCode) 
                                    .FirstOrDefault()
            })
            .ToListAsync();

        return Ok(bookings);
    }

    // --- ADMIN ---
    [HttpGet("all")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<BookingAdminDto>>> GetAllBookings()
    {
        var bookings = await _context.Bookings
            .Include(b => b.Resident)
            .OrderBy(b => b.BookingDate)
            .Select(b => new BookingAdminDto
            {
                Id = b.Id,
                AmenityName = b.AmenityName,
                BookingDate = b.BookingDate,
                Status = b.Status.ToString(),
                CreatedAt = b.CreatedAt,
                ResidentEmail = b.Resident.Email,
                // CORREGIDO: Usamos FullName
                ResidentName = _context.ResidentProfiles
                                    .Where(p => p.UserId == b.ResidentId)
                                    .Select(p => p.FullName) 
                                    .FirstOrDefault(),
                // CORREGIDO: Usamos ResidentCode
                ResidentCode = _context.ResidentProfiles
                                    .Where(p => p.UserId == b.ResidentId)
                                    .Select(p => p.ResidentCode) 
                                    .FirstOrDefault()
            })
            .ToListAsync();

        return Ok(bookings);
    }

    // --- DISPONIBILIDAD ---
    [HttpGet("availability")]
    [Authorize(Roles = "Residente")]
    public async Task<ActionResult<IEnumerable<DateOnly>>> GetBookedDates(
        [FromQuery] string amenityName, [FromQuery] DateOnly month)
    {
        var bookedDates = await _context.Bookings
            .Where(b => 
                b.AmenityName == amenityName &&
                b.Status == BookingStatus.Approved &&
                b.BookingDate.Year == month.Year &&
                b.BookingDate.Month == month.Month
            )
            .Select(b => b.BookingDate)
            .ToListAsync();
        return Ok(bookedDates);
    }

    // --- CREAR RESERVA ---
    [HttpPost]
    [Authorize(Roles = "Residente")]
    public async Task<ActionResult<BookingResidentDto>> CreateBooking(CreateBookingDto createDto)
    {
        var residentId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        bool isAlreadyBooked = await _context.Bookings.AnyAsync(b =>
            b.AmenityName == createDto.AmenityName &&
            b.BookingDate == createDto.BookingDate &&
            b.Status == BookingStatus.Approved
        );

        if (isAlreadyBooked)
        {
            return BadRequest(new { message = "La fecha seleccionada para esta área ya está reservada." });
        }

        var newBooking = new Booking
        {
            ResidentId = residentId,
            AmenityName = createDto.AmenityName,
            BookingDate = createDto.BookingDate,
            Status = BookingStatus.Approved,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Bookings.AddAsync(newBooking);
        await _context.SaveChangesAsync();

        // CORREGIDO: Usamos ResidentCode
        var residentCode = _context.ResidentProfiles
                            .Where(p => p.UserId == residentId)
                            .Select(p => p.ResidentCode)
                            .FirstOrDefault();

        return CreatedAtAction(nameof(GetMyBookings), new { id = newBooking.Id }, new BookingResidentDto
        {
            Id = newBooking.Id,
            AmenityName = newBooking.AmenityName,
            BookingDate = newBooking.BookingDate,
            Status = newBooking.Status.ToString(),
            CreatedAt = newBooking.CreatedAt,
            ResidentCode = residentCode
        });
    }

    // --- CANCELAR RESERVA ---
    [HttpDelete("{id}")]
    [Authorize(Roles = "Residente")]
    public async Task<IActionResult> CancelBooking(Guid id)
    {
        var residentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var booking = await _context.Bookings.FindAsync(id);

        if (booking == null) return NotFound("Reserva no encontrada.");
        if (booking.ResidentId != residentId) return Forbid("No puedes cancelar una reserva que no es tuya.");

        booking.Status = BookingStatus.Cancelled;
        _context.Bookings.Update(booking);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Reserva cancelada exitosamente." });
    }
}