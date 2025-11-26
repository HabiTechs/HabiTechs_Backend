using System;
using System.Linq;
using System.Security.Cryptography;

namespace HabiTechs.Modules.Parcels.Services;

/// <summary>
/// Servicio para generar códigos de recogida cortos y legibles para el residente.
/// </summary>
public class PickupCodeService
{
    private const string Chars = "1234567890ABCDEFGHJKLMNPQRSTUVWXYZ"; // Caracteres sin ambigüedades (sin I, O, 0, 1)
    private const int CodeLength = 6; // Código de 6 caracteres (Ej: 25A7PZ)

    /// <summary>
    /// Genera un código alfanumérico de 6 caracteres.
    /// </summary>
    /// <returns>El código de recogida único.</returns>
    public string GenerateCode()
    {
        var stringChars = new char[CodeLength];
        var random = new Random();

        for (int i = 0; i < CodeLength; i++)
        {
            stringChars[i] = Chars[random.Next(Chars.Length)];
        }

        return new String(stringChars);
    }
}