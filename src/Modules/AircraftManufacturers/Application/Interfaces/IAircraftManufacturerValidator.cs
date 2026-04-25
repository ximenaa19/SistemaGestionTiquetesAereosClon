// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\AircraftManufacturers\Application\Interfaces\IAircraftManufacturerValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.AircraftManufacturers.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.AircraftManufacturers.Application.Interfaces;

public interface IAircraftManufacturerValidator
{
    Task ValidateNameAsync(AircraftManufacturerName name, AircraftManufacturerId? currentId = null);
    Task ValidateCountryExistsAsync(AircraftManufacturerCountryId countryId);
}

