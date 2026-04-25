// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Cities\Application\Interfaces\ICityValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Cities.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Cities.Application.Interfaces;

public interface ICityValidator
{
    Task ValidateRegionExistsAsync(CityRegionId regionId);
    Task ValidateNameAsync(CityName name, CityRegionId regionId, CityId? currentId = null);
}
