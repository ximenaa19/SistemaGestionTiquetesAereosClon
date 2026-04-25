// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Flights\Domain\Repositories\IFlightRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Flights.Domain.Aggregate;
using GestionAerolineas.src.Modules.Flights.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Flights.Domain.Repositories;

public interface IFlightRepository
{
    Task<IEnumerable<Flight>> GetAllAsync();
    Task<Flight?> GetByIdAsync(FlightId id);
    Task<Flight?> GetByCodeAsync(FlightCode code);
    Task<IEnumerable<Flight>> GetByAirlineIdAsync(FlightAirlineId airlineId);
    Task<IEnumerable<Flight>> GetByRouteIdAsync(FlightRouteId routeId);
    Task<IEnumerable<Flight>> GetByStateIdAsync(FlightStateId stateId);
    Task<IEnumerable<Flight>> GetByDepartureDateRangeAsync(DateTime fromInclusive, DateTime toInclusive);
    Task AddAsync(Flight flight);
    Task UpdateAsync(Flight flight);
    Task DeleteAsync(Flight flight);
    Task<bool> ExistsAsync(FlightId id);
    Task<bool> ExistsByNormalizedCodeAsync(string normalizedCode, int? excludingId = null);
    Task<bool> ExistsAircraftOverlapAsync(int aircraftId, DateTime departure, DateTime estimatedArrival, int? excludingId = null);
}

