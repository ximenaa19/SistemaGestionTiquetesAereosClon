// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\RouteStops\Domain\Aggregate\RouteStop.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.RouteStops.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.RouteStops.Domain.Aggregate;

public class RouteStop
{
    public RouteStopId Id { get; private set; }
    public RouteStopRouteId RouteId { get; private set; }
    public RouteStopStopAirportId StopAirportId { get; private set; }
    public RouteStopOrder Order { get; private set; }
    public RouteStopDurationMinutes DurationMinutes { get; private set; }

    private RouteStop(
        RouteStopId id,
        RouteStopRouteId routeId,
        RouteStopStopAirportId stopAirportId,
        RouteStopOrder order,
        RouteStopDurationMinutes durationMinutes)
    {
        Id = id;
        RouteId = routeId;
        StopAirportId = stopAirportId;
        Order = order;
        DurationMinutes = durationMinutes;
    }

    public static RouteStop Create(
        RouteStopId id,
        RouteStopRouteId routeId,
        RouteStopStopAirportId stopAirportId,
        RouteStopOrder order,
        RouteStopDurationMinutes durationMinutes)
    {
        return new RouteStop(id, routeId, stopAirportId, order, durationMinutes);
    }

    public static RouteStop CreateNew(
        RouteStopRouteId routeId,
        RouteStopStopAirportId stopAirportId,
        RouteStopOrder order,
        RouteStopDurationMinutes durationMinutes)
    {
        return new RouteStop(RouteStopId.CreateEmpty(), routeId, stopAirportId, order, durationMinutes);
    }
}

