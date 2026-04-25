// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Fares\Application\Interfaces\IFareValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Fares.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Fares.Application.Interfaces;

public interface IFareValidator
{
    Task ValidateRouteExistsAsync(FareRouteId routeId);
    Task ValidateCabinTypeExistsAsync(FareCabinTypeId cabinTypeId);
    Task ValidatePassengerTypeExistsAsync(FarePassengerTypeId passengerTypeId);
    Task ValidateSeasonExistsAsync(FareSeasonId seasonId);
    Task ValidateUniqueKeysAsync(
        FareRouteId routeId,
        FareCabinTypeId cabinTypeId,
        FarePassengerTypeId passengerTypeId,
        FareSeasonId seasonId,
        FareId? currentId = null);

    void ValidateValidFromBeforeValidUntil(FareValidFromDate validFrom, FareValidUntilDate validUntil);
}

