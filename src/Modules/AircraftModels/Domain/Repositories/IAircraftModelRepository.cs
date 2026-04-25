// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\AircraftModels\Domain\Repositories\IAircraftModelRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.AircraftModels.Domain.Aggregate;
using GestionAerolineas.src.Modules.AircraftModels.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.AircraftModels.Domain.Repositories;

public interface IAircraftModelRepository
{
    Task<IEnumerable<AircraftModel>> GetAllAsync();
    Task<AircraftModel?> GetByIdAsync(AircraftModelId id);
    Task<AircraftModel?> GetByNameAsync(AircraftModelName modelName);
    Task AddAsync(AircraftModel aircraftModel);
    Task UpdateAsync(AircraftModel aircraftModel);
    Task DeleteAsync(AircraftModel aircraftModel);
    Task<bool> ExistsAsync(AircraftModelId id);
}

