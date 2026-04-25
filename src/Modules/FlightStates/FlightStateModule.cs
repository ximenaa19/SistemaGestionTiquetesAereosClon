// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightStates\FlightStateModule.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.FlightStates.Application.Interfaces;
using GestionAerolineas.src.Modules.FlightStates.Application.Services;
using GestionAerolineas.src.Modules.FlightStates.Application.UseCases;
using GestionAerolineas.src.Modules.FlightStates.Infrastructure.Repository;
using GestionAerolineas.src.Modules.FlightStates.UI;
using GestionAerolineas.src.shared.Context;

namespace GestionAerolineas.src.Modules.FlightStates;

public static class FlightStateModule
{
    public static FlightStateMenu Build(AppDbContext context)
    {
        var repository = new FlightStateRepository(context);
        IFlightStateValidator validator = new FlightStateValidator(repository);

        var create = new CreateFlightStateUseCase(repository, validator);
        var getAll = new GetAllFlightStatesUseCase(repository);
        var getById = new GetFlightStateByIdUseCase(repository);
        var getByName = new GetFlightStateByNameUseCase(repository);
        var update = new UpdateFlightStateUseCase(repository, validator);
        var delete = new DeleteFlightStateUseCase(repository);

        return new FlightStateMenu(
            create,
            getAll,
            getById,
            getByName,
            update,
            delete
        );
    }
}
