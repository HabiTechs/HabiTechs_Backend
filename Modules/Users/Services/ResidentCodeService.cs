using HabiTechs.Core.Data;
using Microsoft.EntityFrameworkCore;
using HabiTechs.Modules.Users.Models; // IMPORTANTE: Para ResidentProfile

namespace HabiTechs.Modules.Users.Services;

public class ResidentCodeService
{
    private readonly AppDbContext _context;
    private const int BaseCode = 222111;

    public ResidentCodeService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<string> GenerateNextCodeAsync()
    {
        var lastProfile = await _context.ResidentProfiles
            .OrderByDescending(p => p.ResidentCode)
            .FirstOrDefaultAsync();

        if (lastProfile == null) return BaseCode.ToString();

        if (int.TryParse(lastProfile.ResidentCode, out int lastCode))
        {
            return (lastCode + 1).ToString();
        }

        return BaseCode.ToString();
    }
}