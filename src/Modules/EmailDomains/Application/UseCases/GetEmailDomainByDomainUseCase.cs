// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\EmailDomains\Application\UseCases\GetEmailDomainByDomainUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.EmailDomains.Domain.Aggregate;
using GestionAerolineas.src.Modules.EmailDomains.Domain.Repositories;
using GestionAerolineas.src.Modules.EmailDomains.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.EmailDomains.Application.UseCases;

public class GetEmailDomainByDomainUseCase
{
    private readonly IEmailDomainRepository _repository;

    public GetEmailDomainByDomainUseCase(IEmailDomainRepository repository)
    {
        _repository = repository;
    }

    public async Task<EmailDomain?> ExecuteAsync(string domain)
    {
        var domainVO = EmailDomainValue.Create(domain);
        return await _repository.GetByDomainAsync(domainVO);
    }
}

