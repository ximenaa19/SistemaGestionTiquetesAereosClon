// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Seasons\Domain\Repositories\ISeasonRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Seasons.Domain.Aggregate;
using GestionAerolineas.src.Modules.Seasons.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Seasons.Domain.Repositories;

public interface ISeasonRepository
{
    Task<IEnumerable<Season>> GetAllAsync();
    Task<Season?> GetByIdAsync(SeasonId id);
    Task<Season?> GetByNameAsync(SeasonName name);
    Task AddAsync(Season season);
    Task UpdateAsync(Season season);
    Task DeleteAsync(Season season);
    Task<bool> ExistsAsync(SeasonId id);
}
