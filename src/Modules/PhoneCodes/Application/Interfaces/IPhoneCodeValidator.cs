// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PhoneCodes\Application\Interfaces\IPhoneCodeValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PhoneCodes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.PhoneCodes.Application.Interfaces;

public interface IPhoneCodeValidator
{
    Task ValidateCountryCodeAsync(PhoneCountryCode countryCode, PhoneCodeId? currentId = null);
}

