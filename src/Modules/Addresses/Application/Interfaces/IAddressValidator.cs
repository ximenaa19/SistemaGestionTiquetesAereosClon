// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Addresses\Application\Interfaces\IAddressValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Addresses.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Addresses.Application.Interfaces;

public interface IAddressValidator
{
    Task ValidateRoadTypeExistsAsync(AddressRoadTypeId roadTypeId);
    Task ValidateCityExistsAsync(AddressCityId cityId);
}

