// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\EmailDomains\EmailDomainModule.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.EmailDomains.Application.Interfaces;
using GestionAerolineas.src.Modules.EmailDomains.Application.Services;
using GestionAerolineas.src.Modules.EmailDomains.Application.UseCases;
using GestionAerolineas.src.Modules.EmailDomains.Infrastructure.Repository;
using GestionAerolineas.src.Modules.EmailDomains.UI;
using GestionAerolineas.src.shared.Context;

namespace GestionAerolineas.src.Modules.EmailDomains;

public static class EmailDomainModule
{
    public static EmailDomainMenu Build(AppDbContext context)
    {
        var repository = new EmailDomainRepository(context);
        IEmailDomainValidator validator = new EmailDomainValidator(repository);

        var create = new CreateEmailDomainUseCase(repository, validator);
        var getAll = new GetAllEmailDomainsUseCase(repository);
        var getById = new GetEmailDomainByIdUseCase(repository);
        var getByDomain = new GetEmailDomainByDomainUseCase(repository);
        var update = new UpdateEmailDomainUseCase(repository, validator);
        var delete = new DeleteEmailDomainUseCase(repository);

        return new EmailDomainMenu(
            create,
            getAll,
            getById,
            getByDomain,
            update,
            delete
        );
    }
}

