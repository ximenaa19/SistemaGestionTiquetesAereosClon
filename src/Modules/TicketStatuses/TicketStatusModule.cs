// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\TicketStatuses\TicketStatusModule.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.TicketStatuses.Application.Interfaces;
using GestionAerolineas.src.Modules.TicketStatuses.Application.Services;
using GestionAerolineas.src.Modules.TicketStatuses.Application.UseCases;
using GestionAerolineas.src.Modules.TicketStatuses.Infrastructure.Repository;
using GestionAerolineas.src.Modules.TicketStatuses.UI;
using GestionAerolineas.src.shared.Context;

namespace GestionAerolineas.src.Modules.TicketStatuses;

public static class TicketStatusModule
{
    public static TicketStatusMenu Build(AppDbContext context)
    {
        var repository = new TicketStatusRepository(context);
        ITicketStatusValidator validator = new TicketStatusValidator(repository);

        var create = new CreateTicketStatusUseCase(repository, validator);
        var getAll = new GetAllTicketStatusesUseCase(repository);
        var getById = new GetTicketStatusByIdUseCase(repository);
        var getByName = new GetTicketStatusByNameUseCase(repository);
        var update = new UpdateTicketStatusUseCase(repository, validator);
        var delete = new DeleteTicketStatusUseCase(repository);

        return new TicketStatusMenu(
            create,
            getAll,
            getById,
            getByName,
            update,
            delete
        );
    }
}
