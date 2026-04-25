// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Tickets\Domain\ValueObject\TicketStatusId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Tickets.Domain.ValueObject;

public record TicketStatusId(int Value)
{
    public static TicketStatusId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("estado_tiquete_id no es valido");
        return new TicketStatusId(value);
    }
}

