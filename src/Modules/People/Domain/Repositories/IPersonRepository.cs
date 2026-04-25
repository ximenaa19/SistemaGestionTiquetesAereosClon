// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\People\Domain\Repositories\IPersonRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.People.Domain.Aggregate;
using GestionAerolineas.src.Modules.People.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.People.Domain.Repositories;

public interface IPersonRepository
{
    Task<IEnumerable<Person>> GetAllAsync();
    Task<Person?> GetByIdAsync(PersonId id);
    Task<Person?> GetByDocumentAsync(PersonDocumentTypeId documentTypeId, PersonDocumentNumber documentNumber);
    Task AddAsync(Person person);
    Task UpdateAsync(Person person);
    Task DeleteAsync(Person person);
    Task<bool> ExistsAsync(PersonId id);
    Task<bool> ExistsByNormalizedDocumentInTypeAsync(int documentTypeId, string normalizedDocumentNumber, int? excludingId = null);
}

