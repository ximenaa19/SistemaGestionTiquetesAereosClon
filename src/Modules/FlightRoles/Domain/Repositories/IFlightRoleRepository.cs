// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightRoles\Domain\Repositories\IFlightRoleRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.FlightRoles.Domain.Aggregate;
using GestionAerolineas.src.Modules.FlightRoles.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.FlightRoles.Domain.Repositories;

public interface IFlightRoleRepository
{
    Task<IEnumerable<FlightRole>> GetAllAsync();
    Task<FlightRole?> GetByIdAsync(FlightRoleId id);
    Task<FlightRole?> GetByNameAsync(FlightRoleName name);
    Task AddAsync(FlightRole flightRole);
    Task UpdateAsync(FlightRole flightRole);
    Task DeleteAsync(FlightRole flightRole);
    Task<bool> ExistsAsync(FlightRoleId id);
}

