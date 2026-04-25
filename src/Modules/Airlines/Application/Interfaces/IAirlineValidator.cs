// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Airlines\Application\Interfaces\IAirlineValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Airlines.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Airlines.Application.Interfaces;

public interface IAirlineValidator
{
    Task ValidateOriginCountryExistsAsync(AirlineOriginCountryId originCountryId);
    Task ValidateNameAsync(AirlineName name, AirlineOriginCountryId originCountryId, AirlineId? currentId = null);
    Task ValidateIataCodeAsync(AirlineIataCode iataCode, AirlineId? currentId = null);
}

