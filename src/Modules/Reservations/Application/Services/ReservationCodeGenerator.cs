// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Reservations\Application\Services\ReservationCodeGenerator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System.Security.Cryptography;
using GestionAerolineas.src.Modules.Reservations.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Reservations.Application.Services;

public static class ReservationCodeGenerator
{
    private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    public static ReservationCode Generate()
    {
        Span<byte> data = stackalloc byte[6];
        RandomNumberGenerator.Fill(data);

        var chars = new char[6];
        for (int i = 0; i < chars.Length; i++)
            chars[i] = Alphabet[data[i] % Alphabet.Length];

        return ReservationCode.Create(new string(chars));
    }
}

