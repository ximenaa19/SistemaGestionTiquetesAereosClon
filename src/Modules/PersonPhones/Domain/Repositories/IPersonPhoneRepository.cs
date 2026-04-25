// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PersonPhones\Domain\Repositories\IPersonPhoneRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PersonPhones.Domain.Aggregate;
using GestionAerolineas.src.Modules.PersonPhones.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.PersonPhones.Domain.Repositories;

public interface IPersonPhoneRepository
{
    Task<IEnumerable<PersonPhone>> GetAllAsync();
    Task<PersonPhone?> GetByIdAsync(PersonPhoneId id);
    Task<PersonPhone?> GetByPersonAndCodeAndNumberAsync(PersonPhonePersonId personId, PersonPhoneCodeId phoneCodeId, PersonPhoneNumber phoneNumber);
    Task AddAsync(PersonPhone phone);
    Task UpdateAsync(PersonPhone phone);
    Task DeleteAsync(PersonPhone phone);
    Task<bool> ExistsAsync(PersonPhoneId id);
    Task<bool> ExistsByNormalizedPhoneForPersonAsync(int personId, int phoneCodeId, string normalizedPhoneNumber, int? excludingId = null);
    Task<bool> ExistsPrimaryForPersonAsync(int personId, int? excludingId = null);
}

