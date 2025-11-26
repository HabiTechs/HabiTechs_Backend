using HabiTechs.Modules.Access.Models;
using HabiTechs.Modules.Community.Models;
using HabiTechs.Modules.Bookings.Models; 
using HabiTechs.Modules.Finance.Models;
using HabiTechs.Modules.Users.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HabiTechs.Core.Data;

public class AppDbContext : IdentityDbContext<IdentityUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    // --- MÓDULO ACCESO ---
    public DbSet<Visit> Visits { get; set; }
    public DbSet<Parcel> Parcels { get; set; }
    public DbSet<GateLog> GateLogs { get; set; } 
    
    // --- MÓDULO COMUNIDAD ---
    public DbSet<Announcement> Announcements { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; } 
    
    // --- MÓDULO RESERVAS ---
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<CommonArea> CommonAreas { get; set; } 
    
    // --- MÓDULO FINANZAS (COMPLETO) ---
    public DbSet<Expense> Expenses { get; set; }           // Deudas a residentes
    public DbSet<Payment> Payments { get; set; }           // Historial de pagos (Ingresos)
    public DbSet<PaymentInstruction> PaymentInstructions { get; set; } // Datos QR/Banco del condominio
    public DbSet<OperationalExpense> OperationalExpenses { get; set; } // Gastos del Condominio (Egresos)
    // ------------------------------------

    // --- MÓDULO USUARIOS ---
    public DbSet<ResidentProfile> ResidentProfiles { get; set; }
}