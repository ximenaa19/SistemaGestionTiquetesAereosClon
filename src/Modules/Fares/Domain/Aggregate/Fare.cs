// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Fares\Domain\Aggregate\Fare.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Fares.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Fares.Domain.Aggregate;

public class Fare
{
    public FareId Id { get; private set; }
    public FareRouteId RouteId { get; private set; }
    public FareCabinTypeId CabinTypeId { get; private set; }
    public FarePassengerTypeId PassengerTypeId { get; private set; }
    public FareSeasonId SeasonId { get; private set; }
    public FareBasePrice BasePrice { get; private set; }
    public FareValidFromDate ValidFrom { get; private set; }
    public FareValidUntilDate ValidUntil { get; private set; }

    private Fare(
        FareId id,
        FareRouteId routeId,
        FareCabinTypeId cabinTypeId,
        FarePassengerTypeId passengerTypeId,
        FareSeasonId seasonId,
        FareBasePrice basePrice,
        FareValidFromDate validFrom,
        FareValidUntilDate validUntil)
    {
        Id = id;
        RouteId = routeId;
        CabinTypeId = cabinTypeId;
        PassengerTypeId = passengerTypeId;
        SeasonId = seasonId;
        BasePrice = basePrice;
        ValidFrom = validFrom;
        ValidUntil = validUntil;
    }

    public static Fare Create(
        FareId id,
        FareRouteId routeId,
        FareCabinTypeId cabinTypeId,
        FarePassengerTypeId passengerTypeId,
        FareSeasonId seasonId,
        FareBasePrice basePrice,
        FareValidFromDate validFrom,
        FareValidUntilDate validUntil)
    {
        return new Fare(id, routeId, cabinTypeId, passengerTypeId, seasonId, basePrice, validFrom, validUntil);
    }

    public static Fare CreateNew(
        FareRouteId routeId,
        FareCabinTypeId cabinTypeId,
        FarePassengerTypeId passengerTypeId,
        FareSeasonId seasonId,
        FareBasePrice basePrice,
        FareValidFromDate validFrom,
        FareValidUntilDate validUntil)
    {
        return new Fare(FareId.CreateEmpty(), routeId, cabinTypeId, passengerTypeId, seasonId, basePrice, validFrom, validUntil);
    }
}

