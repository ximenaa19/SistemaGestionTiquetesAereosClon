// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PersonEmails\Domain\Repositories\IPersonEmailRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PersonEmails.Domain.Aggregate;
using GestionAerolineas.src.Modules.PersonEmails.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.PersonEmails.Domain.Repositories;

public interface IPersonEmailRepository
{
    Task<IEnumerable<PersonEmail>> GetAllAsync();
    Task<PersonEmail?> GetByIdAsync(PersonEmailId id);
    Task<PersonEmail?> GetByPersonAndUserAndDomainAsync(PersonEmailPersonId personId, PersonEmailUser user, PersonEmailDomainId emailDomainId);
    Task AddAsync(PersonEmail email);
    Task UpdateAsync(PersonEmail email);
    Task DeleteAsync(PersonEmail email);
    Task<bool> ExistsAsync(PersonEmailId id);
    Task<bool> ExistsByNormalizedUserAndDomainForPersonAsync(int personId, string normalizedUser, int emailDomainId, int? excludingId = null);
    Task<bool> ExistsPrimaryForPersonAsync(int personId, int? excludingId = null);
}

