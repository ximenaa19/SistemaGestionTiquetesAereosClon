// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\SeatLocationTypes\Domain\Repositories\ISeatLocationTypeRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.SeatLocationTypes.Domain.Aggregate;
using GestionAerolineas.src.Modules.SeatLocationTypes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.SeatLocationTypes.Domain.Repositories;

public interface ISeatLocationTypeRepository
{
    Task<IEnumerable<SeatLocationType>> GetAllAsync();
    Task<SeatLocationType?> GetByIdAsync(SeatLocationTypeId id);
    Task<SeatLocationType?> GetByNameAsync(SeatLocationTypeName name);
    Task<int> CountAsync();
    Task AddAsync(SeatLocationType seatLocationType);
    Task UpdateAsync(SeatLocationType seatLocationType);
    Task DeleteAsync(SeatLocationType seatLocationType);
    Task<bool> ExistsAsync(SeatLocationTypeId id);
}

