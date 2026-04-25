// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Regions\Aplication\Interfaces\IRegionValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Regions.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Regions.Application.Interfaces;

public interface IRegionValidator
{
    Task ValidateCountryExistsAsync(RegionCountryId countryId);
    Task ValidateNameAsync(RegionName name, RegionCountryId countryId, RegionId? currentId = null);
}

