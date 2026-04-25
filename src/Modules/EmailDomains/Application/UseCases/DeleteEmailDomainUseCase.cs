// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\EmailDomains\Application\UseCases\DeleteEmailDomainUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.EmailDomains.Domain.Repositories;
using GestionAerolineas.src.Modules.EmailDomains.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.EmailDomains.Application.UseCases;

public class DeleteEmailDomainUseCase
{
    private readonly IEmailDomainRepository _repository;

    public DeleteEmailDomainUseCase(IEmailDomainRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var emailDomainId = EmailDomainId.Create(id);
        var emailDomain = await _repository.GetByIdAsync(emailDomainId);

        if (emailDomain is null)
        {
            throw new KeyNotFoundException($"EmailDomain con id '{emailDomainId.Value}' no existe.");
        }

        await _repository.DeleteAsync(emailDomain);
    }
}

