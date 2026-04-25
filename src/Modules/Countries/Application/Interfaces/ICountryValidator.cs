// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Countries\Application\Interfaces\ICountryValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Countries.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Countries.Application.Interfaces;

public interface ICountryValidator
{
    Task ValidateIsoCodeAsync(CountryCodigoIso isoCode, CountryId? currentId = null);
    Task ValidateContinentExistsAsync(CountryContinentId continentId);
}

