// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Reservations\Application\Interfaces\IReservationValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Reservations.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Reservations.Application.Interfaces;

public interface IReservationValidator
{
    Task ValidateCustomerExistsAsync(ReservationCustomerId customerId);
    Task ValidateStatusExistsAsync(ReservationStatusId statusId);
    Task ValidateCodeUniqueAsync(ReservationCode code, ReservationId? currentId = null);
    void ValidateExpiresAt(ReservationReservedAt reservedAt, ReservationExpiresAt expiresAt);
    Task ValidateStatusTransitionAsync(ReservationStatusId currentStatusId, ReservationStatusId newStatusId);
}

