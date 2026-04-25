// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\AirportAirline\Application\Interfaces\IAirportAirlineValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.AirportAirline.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.AirportAirline.Application.Interfaces;

public interface IAirportAirlineValidator
{
    Task ValidateAirportExistsAsync(AirportAirlineAirportId airportId);
    Task ValidateAirlineExistsAsync(AirportAirlineAirlineId airlineId);
    Task ValidateUniquePairAsync(AirportAirlineAirportId airportId, AirportAirlineAirlineId airlineId, AirportAirlineId? currentId = null);
    Task ValidateDatesAsync(AirportAirlineStartDate startDate, AirportAirlineEndDate endDate);
}

