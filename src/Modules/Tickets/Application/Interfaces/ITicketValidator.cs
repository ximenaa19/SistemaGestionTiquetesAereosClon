// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Tickets\Application\Interfaces\ITicketValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Tickets.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Tickets.Application.Interfaces;

public interface ITicketValidator
{
    Task ValidateReservationPassengerExistsAsync(TicketReservationPassengerId reservationPassengerId);
    Task ValidateReservationPassengerIsUniqueAsync(TicketReservationPassengerId reservationPassengerId, TicketId? excludingId = null);
    Task ValidateTicketStatusExistsAsync(TicketStatusId statusId);
    Task ValidateReservationIsConfirmadaAsync(TicketReservationPassengerId reservationPassengerId);
    Task ValidateTicketCodeUniqueAsync(TicketCode code, TicketId? excludingId = null);
}

