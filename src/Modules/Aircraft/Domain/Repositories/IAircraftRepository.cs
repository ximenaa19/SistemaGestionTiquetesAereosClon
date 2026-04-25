// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Aircraft\Domain\Repositories\IAircraftRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Aircraft.Domain.Aggregate;
using GestionAerolineas.src.Modules.Aircraft.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Aircraft.Domain.Repositories;

public interface IAircraftRepository
{
    Task<IEnumerable<AircraftAggregate>> GetAllAsync();
    Task<AircraftAggregate?> GetByIdAsync(AircraftId id);
    Task<AircraftAggregate?> GetByRegistrationAsync(AircraftRegistration registration);
    Task AddAsync(AircraftAggregate aircraft);
    Task UpdateAsync(AircraftAggregate aircraft);
    Task DeleteAsync(AircraftAggregate aircraft);
    Task<bool> ExistsAsync(AircraftId id);
    Task<bool> ExistsByNormalizedRegistrationAsync(string normalizedRegistration, int? excludingId = null);
}

