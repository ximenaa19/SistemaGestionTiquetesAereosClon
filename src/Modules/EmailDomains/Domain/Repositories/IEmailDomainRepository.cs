// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\EmailDomains\Domain\Repositories\IEmailDomainRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.EmailDomains.Domain.Aggregate;
using GestionAerolineas.src.Modules.EmailDomains.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.EmailDomains.Domain.Repositories;

public interface IEmailDomainRepository
{
    Task<IEnumerable<EmailDomain>> GetAllAsync();
    Task<EmailDomain?> GetByIdAsync(EmailDomainId id);
    Task<EmailDomain?> GetByDomainAsync(EmailDomainValue domain);
    Task AddAsync(EmailDomain emailDomain);
    Task UpdateAsync(EmailDomain emailDomain);
    Task DeleteAsync(EmailDomain emailDomain);
    Task<bool> ExistsAsync(EmailDomainId id);
}

