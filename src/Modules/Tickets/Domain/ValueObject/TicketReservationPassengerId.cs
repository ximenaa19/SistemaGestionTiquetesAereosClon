// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Tickets\Domain\ValueObject\TicketReservationPassengerId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Tickets.Domain.ValueObject;

public record TicketReservationPassengerId(int Value)
{
    public static TicketReservationPassengerId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("reserva_pasajero_id no es valido");
        return new TicketReservationPassengerId(value);
    }
}

