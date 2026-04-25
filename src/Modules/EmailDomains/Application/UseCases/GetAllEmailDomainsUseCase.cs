// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\EmailDomains\Application\UseCases\GetAllEmailDomainsUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.EmailDomains.Domain.Aggregate;
using GestionAerolineas.src.Modules.EmailDomains.Domain.Repositories;

namespace GestionAerolineas.src.Modules.EmailDomains.Application.UseCases;

public class GetAllEmailDomainsUseCase
{
    private readonly IEmailDomainRepository _repository;

    public GetAllEmailDomainsUseCase(IEmailDomainRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<EmailDomain>> ExecuteAsync()
    {
        return await _repository.GetAllAsync();
    }
}

