// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Cities\Domain\Repositories\ICityRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Cities.Domain.Aggregate;
using GestionAerolineas.src.Modules.Cities.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Cities.Domain.Repositories;

public interface ICityRepository
{
    Task<IEnumerable<City>> GetAllAsync();
    Task<City?> GetByIdAsync(CityId id);
    Task<City?> GetByNameAsync(CityName name);
    Task AddAsync(City city);
    Task UpdateAsync(City city);
    Task DeleteAsync(City city);
    Task<bool> ExistsAsync(CityId id);
    Task<bool> ExistsByNormalizedNameInRegionAsync(string normalizedName, int regionId, int? excludingId = null);
}
