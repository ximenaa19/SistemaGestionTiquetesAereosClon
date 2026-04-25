// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Airlines\Domain\Repositories\IAirlineRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Airlines.Domain.Aggregate;
using GestionAerolineas.src.Modules.Airlines.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Airlines.Domain.Repositories;

public interface IAirlineRepository
{
    Task<IEnumerable<Airline>> GetAllAsync();
    Task<Airline?> GetByIdAsync(AirlineId id);
    Task<Airline?> GetByNameAsync(AirlineName name);
    Task AddAsync(Airline airline);
    Task UpdateAsync(Airline airline);
    Task DeleteAsync(Airline airline);
    Task<bool> ExistsAsync(AirlineId id);
    Task<bool> ExistsByNormalizedNameInOriginCountryAsync(string normalizedName, int originCountryId, int? excludingId = null);
    Task<bool> ExistsByNormalizedIataCodeAsync(string normalizedIataCode, int? excludingId = null);
}

