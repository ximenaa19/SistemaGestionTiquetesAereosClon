// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\EmailDomains\Application\Services\EmailDomainValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.EmailDomains.Application.Interfaces;
using GestionAerolineas.src.Modules.EmailDomains.Domain.Repositories;
using GestionAerolineas.src.Modules.EmailDomains.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.EmailDomains.Application.Services;

public class EmailDomainValidator : IEmailDomainValidator
{
    private readonly IEmailDomainRepository _repository;

    public EmailDomainValidator(IEmailDomainRepository repository)
    {
        _repository = repository;
    }

    public async Task ValidateDomainAsync(EmailDomainValue domain)
    {
        var existing = await _repository.GetByDomainAsync(domain);

        if (existing != null)
            throw new Exception("Ya existe un EmailDomain con ese dominio");
    }
}

