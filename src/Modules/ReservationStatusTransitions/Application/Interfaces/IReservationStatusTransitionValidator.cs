// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationStatusTransitions\Application\Interfaces\IReservationStatusTransitionValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System;
using GestionAerolineas.src.Modules.ReservationStatusTransitions.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.ReservationStatusTransitions.Application.Interfaces;

public interface IReservationStatusTransitionValidator
{
    Task ValidatePairAsync(
        ReservationStatusOriginId originStatusId,
        ReservationStatusDestinationId destinationStatusId,
        ReservationStatusTransitionId? currentId = null
    );

}
