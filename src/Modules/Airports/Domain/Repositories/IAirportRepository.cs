// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Airports\Domain\Repositories\IAirportRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Airports.Domain.Aggregate;
using GestionAerolineas.src.Modules.Airports.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Airports.Domain.Repositories;

public interface IAirportRepository
{
    Task<IEnumerable<Airport>> GetAllAsync();
    Task<Airport?> GetByIdAsync(AirportId id);
    Task<Airport?> GetByNameAsync(AirportName name);
    Task AddAsync(Airport airport);
    Task UpdateAsync(Airport airport);
    Task DeleteAsync(Airport airport);
    Task<bool> ExistsAsync(AirportId id);
    Task<bool> ExistsByNormalizedNameInCityAsync(string normalizedName, int cityId, int? excludingId = null);
    Task<bool> ExistsByNormalizedIataCodeAsync(string normalizedIataCode, int? excludingId = null);
    Task<bool> ExistsByNormalizedIcaoCodeAsync(string normalizedIcaoCode, int? excludingId = null);
}
