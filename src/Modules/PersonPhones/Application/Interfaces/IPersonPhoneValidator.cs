// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PersonPhones\Application\Interfaces\IPersonPhoneValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PersonPhones.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.PersonPhones.Application.Interfaces;

public interface IPersonPhoneValidator
{
    Task ValidatePersonExistsAsync(PersonPhonePersonId personId);
    Task ValidatePhoneCodeExistsAsync(PersonPhoneCodeId phoneCodeId);
    Task ValidateUniquePhoneForPersonAsync(PersonPhonePersonId personId, PersonPhoneCodeId phoneCodeId, PersonPhoneNumber phoneNumber, PersonPhoneId? currentId = null);
    Task ValidatePrimaryPhoneAsync(PersonPhonePersonId personId, PersonPhoneIsPrimary isPrimary, PersonPhoneId? currentId = null);
}

