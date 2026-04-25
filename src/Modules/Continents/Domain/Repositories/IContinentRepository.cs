// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Continents\Domain\Repositories\IContinentRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Continents.Domain.Aggregate;
using GestionAerolineas.src.Modules.Continents.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Continents.Domain.Repositories;

public interface IContinentRepository
{
    Task<IEnumerable<Continent>> GetAllAsync();
    Task<Continent?> GetByIdAsync(ContinentId id);
    Task<Continent?> GetByNameAsync(ContinentName name);
    Task AddAsync(Continent continent);
    Task UpdateAsync(Continent continent);
    Task DeleteAsync(Continent continent);
    Task<bool> ExistsAsync(ContinentId id);
}

