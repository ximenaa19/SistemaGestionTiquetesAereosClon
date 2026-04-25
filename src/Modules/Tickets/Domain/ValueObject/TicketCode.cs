// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Tickets\Domain\ValueObject\TicketCode.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Tickets.Domain.ValueObject;

public record TicketCode(string Value)
{
    public static TicketCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("codigo_tiquete es obligatorio");

        var trimmed = value.Trim();
        if (trimmed.Length > 30)
            throw new ArgumentException("codigo_tiquete excede 30 caracteres");

        return new TicketCode(trimmed);
    }

    public static string Normalize(string value) => (value ?? string.Empty).Trim().ToUpperInvariant();
}

