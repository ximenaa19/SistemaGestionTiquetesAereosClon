// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\AircraftModels\Application\Interfaces\IAircraftModelValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.AircraftModels.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.AircraftModels.Application.Interfaces;

public interface IAircraftModelValidator
{
    Task ValidateManufacturerExistsAsync(AircraftManufacturerId manufacturerId);
    Task ValidateModelNameAsync(AircraftModelName modelName, AircraftModelId? currentId = null);
}

