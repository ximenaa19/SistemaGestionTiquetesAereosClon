// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Airports\Application\Interfaces\IAirportValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Airports.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Airports.Application.Interfaces;

public interface IAirportValidator
{
    Task ValidateCityExistsAsync(AirportCityId cityId);
    Task ValidateNameAsync(AirportName name, AirportCityId cityId, AirportId? currentId = null);
    Task ValidateIataCodeAsync(AirportIataCode iataCode, AirportId? currentId = null);
    Task ValidateIcaoCodeAsync(AirportIcaoCode? icaoCode, AirportId? currentId = null);
}
