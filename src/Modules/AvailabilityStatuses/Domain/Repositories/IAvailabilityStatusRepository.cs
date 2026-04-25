// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\AvailabilityStatuses\Domain\Repositories\IAvailabilityStatusRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.AvailabilityStatuses.Domain.Aggregate;
using GestionAerolineas.src.Modules.AvailabilityStatuses.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.AvailabilityStatuses.Domain.Repositories;

public interface IAvailabilityStatusRepository
{
    Task<IEnumerable<AvailabilityStatus>> GetAllAsync();
    Task<AvailabilityStatus?> GetByIdAsync(AvailabilityStatusId id);
    Task<AvailabilityStatus?> GetByNameAsync(AvailabilityStatusName name);
    Task AddAsync(AvailabilityStatus availabilityStatus);
    Task UpdateAsync(AvailabilityStatus availabilityStatus);
    Task DeleteAsync(AvailabilityStatus availabilityStatus);
    Task<bool> ExistsAsync(AvailabilityStatusId id);
}
