// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Checkins\Domain\ValueObject\CheckinBoardingPassNumber.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Checkins.Domain.ValueObject;

public record CheckinBoardingPassNumber(string Value)
{
    public static CheckinBoardingPassNumber Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("numero_tarjeta_embarque es obligatorio");

        var trimmed = value.Trim();
        if (trimmed.Length > 20)
            throw new ArgumentException("numero_tarjeta_embarque excede 20 caracteres");

        return new CheckinBoardingPassNumber(trimmed);
    }

    public static string Normalize(string value) => (value ?? string.Empty).Trim().ToUpperInvariant();
}

