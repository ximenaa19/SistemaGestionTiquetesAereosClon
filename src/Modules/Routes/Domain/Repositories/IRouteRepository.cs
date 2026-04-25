// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Routes\Domain\Repositories\IRouteRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Routes.Domain.Aggregate;
using GestionAerolineas.src.Modules.Routes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Routes.Domain.Repositories;

public interface IRouteRepository
{
    Task<IEnumerable<Route>> GetAllAsync();
    Task<Route?> GetByIdAsync(RouteId id);
    Task<Route?> GetByOriginAndDestinationAsync(RouteAirportId originAirportId, RouteAirportId destinationAirportId);
    Task AddAsync(Route route);
    Task UpdateAsync(Route route);
    Task DeleteAsync(Route route);
    Task<bool> ExistsAsync(RouteId id);
    Task<bool> ExistsByOriginAndDestinationAsync(int originAirportId, int destinationAirportId, int? excludingId = null);
}

