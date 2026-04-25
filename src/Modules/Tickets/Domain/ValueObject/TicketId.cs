// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Tickets\Domain\ValueObject\TicketId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Tickets.Domain.ValueObject;

public record TicketId(int Value)
{
    public static TicketId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El id del tiquete no es valido");
        return new TicketId(value);
    }

    public static TicketId CreateEmpty() => new(0);
}

