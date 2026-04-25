// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Routes\Domain\Aggregate\Route.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Routes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Routes.Domain.Aggregate;

public class Route
{
    public RouteId Id { get; private set; }
    public RouteAirportId OriginAirportId { get; private set; }
    public RouteAirportId DestinationAirportId { get; private set; }
    public RouteDistanceKm DistanceKm { get; private set; }
    public RouteEstimatedDurationMinutes EstimatedDurationMin { get; private set; }

    private Route(
        RouteId id,
        RouteAirportId originAirportId,
        RouteAirportId destinationAirportId,
        RouteDistanceKm distanceKm,
        RouteEstimatedDurationMinutes estimatedDurationMin)
    {
        Id = id;
        OriginAirportId = originAirportId;
        DestinationAirportId = destinationAirportId;
        DistanceKm = distanceKm;
        EstimatedDurationMin = estimatedDurationMin;
    }

    public static Route Create(
        RouteId id,
        RouteAirportId originAirportId,
        RouteAirportId destinationAirportId,
        RouteDistanceKm distanceKm,
        RouteEstimatedDurationMinutes estimatedDurationMin)
    {
        return new Route(id, originAirportId, destinationAirportId, distanceKm, estimatedDurationMin);
    }

    public static Route CreateNew(
        RouteAirportId originAirportId,
        RouteAirportId destinationAirportId,
        RouteDistanceKm distanceKm,
        RouteEstimatedDurationMinutes estimatedDurationMin)
    {
        return new Route(RouteId.CreateEmpty(), originAirportId, destinationAirportId, distanceKm, estimatedDurationMin);
    }
}

