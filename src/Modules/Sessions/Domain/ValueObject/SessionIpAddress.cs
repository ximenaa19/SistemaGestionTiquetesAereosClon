// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Sessions\Domain\ValueObject\SessionIpAddress.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System.Net;

namespace GestionAerolineas.src.Modules.Sessions.Domain.ValueObject;

public sealed record SessionIpAddress
{
    public string? Value { get; }

    private SessionIpAddress(string? value)
    {
        Value = value;
    }

    public static SessionIpAddress Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return new SessionIpAddress((string?)null);

        var trimmed = value.Trim();

        if (trimmed.Length > 45)
            throw new ArgumentException("La ip no puede superar 45 caracteres");

        if (!IPAddress.TryParse(trimmed, out _))
            throw new ArgumentException("La ip no tiene un formato valido");

        return new SessionIpAddress(trimmed);
    }
}
