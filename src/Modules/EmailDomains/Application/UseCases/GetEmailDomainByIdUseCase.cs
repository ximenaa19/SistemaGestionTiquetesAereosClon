// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\EmailDomains\Application\UseCases\GetEmailDomainByIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.EmailDomains.Domain.Aggregate;
using GestionAerolineas.src.Modules.EmailDomains.Domain.Repositories;
using GestionAerolineas.src.Modules.EmailDomains.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.EmailDomains.Application.UseCases;

public class GetEmailDomainByIdUseCase
{
    private readonly IEmailDomainRepository _repository;

    public GetEmailDomainByIdUseCase(IEmailDomainRepository repository)
    {
        _repository = repository;
    }

    public async Task<EmailDomain?> ExecuteAsync(int id)
    {
        var idVO = EmailDomainId.Create(id);
        return await _repository.GetByIdAsync(idVO);
    }
}

