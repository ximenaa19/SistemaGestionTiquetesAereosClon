// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Routes\Application\Interfaces\IRouteValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Routes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Routes.Application.Interfaces;

public interface IRouteValidator
{
    Task ValidateAirportExistsAsync(RouteAirportId airportId);
    Task ValidateUniquePairAsync(RouteAirportId originAirportId, RouteAirportId destinationAirportId, RouteId? currentId = null);
    Task ValidateDifferentAirportsAsync(RouteAirportId originAirportId, RouteAirportId destinationAirportId);
}

