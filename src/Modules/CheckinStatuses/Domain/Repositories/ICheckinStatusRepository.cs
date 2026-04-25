// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\CheckinStatuses\Domain\Repositories\ICheckinStatusRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.CheckinStatuses.Domain.Aggregate;
using GestionAerolineas.src.Modules.CheckinStatuses.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.CheckinStatuses.Domain.Repositories;

public interface ICheckinStatusRepository
{
    Task<IEnumerable<CheckinStatus>> GetAllAsync();
    Task<CheckinStatus?> GetByIdAsync(CheckinStatusId id);
    Task<CheckinStatus?> GetByNameAsync(CheckinStatusName name);
    Task AddAsync(CheckinStatus checkinStatus);
    Task UpdateAsync(CheckinStatus checkinStatus);
    Task DeleteAsync(CheckinStatus checkinStatus);
    Task<bool> ExistsAsync(CheckinStatusId id);
}
