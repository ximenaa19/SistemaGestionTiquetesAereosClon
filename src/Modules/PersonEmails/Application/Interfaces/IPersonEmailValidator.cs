// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PersonEmails\Application\Interfaces\IPersonEmailValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PersonEmails.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.PersonEmails.Application.Interfaces;

public interface IPersonEmailValidator
{
    Task ValidatePersonExistsAsync(PersonEmailPersonId personId);
    Task ValidateEmailDomainExistsAsync(PersonEmailDomainId emailDomainId);
    Task ValidateUniqueEmailForPersonAsync(PersonEmailPersonId personId, PersonEmailUser user, PersonEmailDomainId emailDomainId, PersonEmailId? currentId = null);
    Task ValidatePrimaryEmailAsync(PersonEmailPersonId personId, PersonEmailIsPrimary isPrimary, PersonEmailId? currentId = null);
}

