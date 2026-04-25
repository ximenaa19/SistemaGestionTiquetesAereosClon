// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\RoadTypes\Domain\Repositories\IRoadTypeRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System;
using GestionAerolineas.src.Modules.RoadTypes.Domain.Aggregate;
using GestionAerolineas.src.Modules.RoadTypes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.RoadTypes.Domain.Repositories;

public interface IRoadTypeRepository
{
    Task<IEnumerable<RoadType>> GetAllAsync();
    Task<RoadType?> GetByIdAsync(RoadTypeId id);
    Task<RoadType?> GetByNameAsync(RoadTypeName name);
    Task AddAsync(RoadType roadType);
    Task UpdateAsync(RoadType roadType);
    Task DeleteAsync(RoadType roadType);
    Task<bool> ExistsAsync(RoadTypeId id);

}
     