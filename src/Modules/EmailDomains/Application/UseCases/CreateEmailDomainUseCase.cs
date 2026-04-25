// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\EmailDomains\Application\UseCases\CreateEmailDomainUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.EmailDomains.Application.Interfaces;
using GestionAerolineas.src.Modules.EmailDomains.Domain.Aggregate;
using GestionAerolineas.src.Modules.EmailDomains.Domain.Repositories;
using GestionAerolineas.src.Modules.EmailDomains.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.EmailDomains.Application.UseCases;

public class CreateEmailDomainUseCase
{
    private readonly IEmailDomainRepository _repository;
    private readonly IEmailDomainValidator _validator;

    public CreateEmailDomainUseCase(
        IEmailDomainRepository repository,
        IEmailDomainValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(string domain)
    {
        var domainVO = EmailDomainValue.Create(domain);

        await _validator.ValidateDomainAsync(domainVO);

        var entity = EmailDomain.CreateNew(domainVO);

        await _repository.AddAsync(entity);
    }
}

