// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Fares\Domain\Repositories\IFareRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Fares.Domain.Aggregate;
using GestionAerolineas.src.Modules.Fares.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Fares.Domain.Repositories;

public interface IFareRepository
{
    Task<IEnumerable<Fare>> GetAllAsync();
    Task<Fare?> GetByIdAsync(FareId id);
    Task<IEnumerable<Fare>> GetByRouteIdAsync(FareRouteId routeId);
    Task<Fare?> GetByKeysAsync(FareRouteId routeId, FareCabinTypeId cabinTypeId, FarePassengerTypeId passengerTypeId, FareSeasonId seasonId);
    Task AddAsync(Fare fare);
    Task UpdateAsync(Fare fare);
    Task DeleteAsync(Fare fare);
    Task<bool> ExistsAsync(FareId id);
    Task<bool> ExistsByKeysAsync(int routeId, int cabinTypeId, int passengerTypeId, int seasonId, int? excludingId = null);
}

