// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Regions\Domain\Repositories\IRegionRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Regions.Domain.Aggregate;
using GestionAerolineas.src.Modules.Regions.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Regions.Domain.Repositories;

public interface IRegionRepository
{
    Task<IEnumerable<Region>> GetAllAsync();
    Task<Region?> GetByIdAsync(RegionId id);
    Task<Region?> GetByNameAsync(RegionName name);
    Task AddAsync(Region region);
    Task UpdateAsync(Region region);
    Task DeleteAsync(Region region);
    Task<bool> ExistsAsync(RegionId id);
    Task<bool> ExistsByNormalizedNameInCountryAsync(string normalizedName, int countryId, int? excludingId = null);
}
