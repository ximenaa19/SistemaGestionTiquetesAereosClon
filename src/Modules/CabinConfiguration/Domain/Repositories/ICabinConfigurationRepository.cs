// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\CabinConfiguration\Domain\Repositories\ICabinConfigurationRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.CabinConfiguration.Domain.Aggregate;
using GestionAerolineas.src.Modules.CabinConfiguration.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.CabinConfiguration.Domain.Repositories;

public interface ICabinConfigurationRepository
{
    Task<IEnumerable<CabinConfigurationAggregate>> GetAllAsync();
    Task<CabinConfigurationAggregate?> GetByIdAsync(CabinConfigurationId id);
    Task<IEnumerable<CabinConfigurationAggregate>> GetByAircraftIdAsync(CabinConfigurationAircraftId aircraftId);
    Task<CabinConfigurationAggregate?> GetByAircraftAndCabinTypeAsync(CabinConfigurationAircraftId aircraftId, CabinConfigurationCabinTypeId cabinTypeId);
    Task AddAsync(CabinConfigurationAggregate cabinConfiguration);
    Task UpdateAsync(CabinConfigurationAggregate cabinConfiguration);
    Task DeleteAsync(CabinConfigurationAggregate cabinConfiguration);
    Task<bool> ExistsAsync(CabinConfigurationId id);
    Task<bool> ExistsByAircraftAndCabinTypeAsync(int aircraftId, int cabinTypeId, int? excludingId = null);
}
