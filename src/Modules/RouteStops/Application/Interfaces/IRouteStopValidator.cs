// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\RouteStops\Application\Interfaces\IRouteStopValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.RouteStops.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.RouteStops.Application.Interfaces;

public interface IRouteStopValidator
{
    Task ValidateRouteExistsAsync(RouteStopRouteId routeId);
    Task ValidateStopAirportExistsAsync(RouteStopStopAirportId stopAirportId);
    Task ValidateUniqueOrderInRouteAsync(RouteStopRouteId routeId, RouteStopOrder order, RouteStopId? currentId = null);
    Task ValidateNoDuplicateStopAirportInRouteAsync(RouteStopRouteId routeId, RouteStopStopAirportId stopAirportId, RouteStopId? currentId = null);
    Task ValidateStopAirportNotOriginOrDestinationAsync(RouteStopRouteId routeId, RouteStopStopAirportId stopAirportId);
}

