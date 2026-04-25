// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Tickets\Domain\ValueObject\TicketCreatedAt.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Tickets.Domain.ValueObject;

public sealed record TicketCreatedAt
{
    public DateTime? Value { get; }

    private TicketCreatedAt(DateTime? value)
    {
        Value = value;
    }

    public static TicketCreatedAt CreateOptional(DateTime? value)
    {
        return new TicketCreatedAt(value);
    }
}

