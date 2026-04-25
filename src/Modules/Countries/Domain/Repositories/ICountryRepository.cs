// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Countries\Domain\Repositories\ICountryRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Countries.Domain.Aggregate;
using GestionAerolineas.src.Modules.Countries.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Countries.Domain.Repositories;

public interface ICountryRepository
{
    Task<IEnumerable<Country>> GetAllAsync();
    Task<Country?> GetByIdAsync(CountryId id);
    Task<Country?> GetByNameAsync(CountryName name);
    Task<Country?> GetByIsoCodeAsync(CountryCodigoIso isoCode);
    Task AddAsync(Country country);
    Task UpdateAsync(Country country);
    Task DeleteAsync(Country country);
    Task<bool> ExistsAsync(CountryId id);
}

