// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\AirportAirline\Domain\Repositories\IAirportAirlineRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.AirportAirline.Domain.Aggregate;
using GestionAerolineas.src.Modules.AirportAirline.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.AirportAirline.Domain.Repositories;

public interface IAirportAirlineRepository
{
    Task<IEnumerable<AirportAirlineRelation>> GetAllAsync();
    Task<AirportAirlineRelation?> GetByIdAsync(AirportAirlineId id);
    Task<AirportAirlineRelation?> GetByAirportAndAirlineAsync(AirportAirlineAirportId airportId, AirportAirlineAirlineId airlineId);
    Task AddAsync(AirportAirlineRelation relation);
    Task UpdateAsync(AirportAirlineRelation relation);
    Task DeleteAsync(AirportAirlineRelation relation);
    Task<bool> ExistsAsync(AirportAirlineId id);
    Task<bool> ExistsByAirportAndAirlineAsync(int airportId, int airlineId, int? excludingId = null);
}

