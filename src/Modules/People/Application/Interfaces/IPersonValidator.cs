// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\People\Application\Interfaces\IPersonValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.People.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.People.Application.Interfaces;

public interface IPersonValidator
{
    Task ValidateDocumentTypeExistsAsync(PersonDocumentTypeId documentTypeId);
    Task ValidateAddressExistsAsync(PersonAddressId addressId);
    Task ValidateUniqueDocumentAsync(PersonDocumentTypeId documentTypeId, PersonDocumentNumber documentNumber, PersonId? currentId = null);
}

