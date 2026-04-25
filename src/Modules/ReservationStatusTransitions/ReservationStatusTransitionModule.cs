// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationStatusTransitions\ReservationStatusTransitionModule.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.ReservationStatusTransitions.Application.Interfaces;
using GestionAerolineas.src.Modules.ReservationStatusTransitions.Application.Services;
using GestionAerolineas.src.Modules.ReservationStatusTransitions.Application.UseCases;
using GestionAerolineas.src.Modules.ReservationStatusTransitions.Infrastructure.Repository;
using GestionAerolineas.src.Modules.ReservationStatusTransitions.UI;
using GestionAerolineas.src.Modules.ReservationStatuses.Application.UseCases;
using GestionAerolineas.src.Modules.ReservationStatuses.Infrastructure.Repository;
using GestionAerolineas.src.shared.Context;

namespace GestionAerolineas.src.Modules.ReservationStatusTransitions;

public static class ReservationStatusTransitionModule
{
    public static ReservationStatusTransitionMenu Build(AppDbContext context)
    {
        var repository = new ReservationStatusTransitionRepository(context);
        IReservationStatusTransitionValidator validator = new ReservationStatusTransitionValidator(repository);

        var create = new CreateReservationStatusTransitionUseCase(repository, validator);
        var getAll = new GetAllReservationStatusTransitionsUseCase(repository);
        var getById = new GetReservationStatusTransitionByIdUseCase(repository);
        var getByPair = new GetReservationStatusTransitionByPairUseCase(repository);
        var update = new UpdateReservationStatusTransitionUseCase(repository, validator);
        var delete = new DeleteReservationStatusTransitionUseCase(repository);

        var statusRepository = new ReservationStatusRepository(context);
        var getAllStatuses = new GetAllReservationStatusesUseCase(statusRepository);

        return new ReservationStatusTransitionMenu(
            create,
            getAll,
            getById,
            getByPair,
            update,
            delete,
            getAllStatuses
        );
    }
}
