// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightStates\Domain\Repositories\IFlightStateRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.FlightStates.Domain.Aggregate;
using GestionAerolineas.src.Modules.FlightStates.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.FlightStates.Domain.Repositories;

public interface IFlightStateRepository
{
    Task<IEnumerable<FlightState>> GetAllAsync();
    Task<FlightState?> GetByIdAsync(FlightStateId id);
    Task<FlightState?> GetByNameAsync(FlightStateName name);
    Task AddAsync(FlightState flightState);
    Task UpdateAsync(FlightState flightState);
    Task DeleteAsync(FlightState flightState);
    Task<bool> ExistsAsync(FlightStateId id);
}
