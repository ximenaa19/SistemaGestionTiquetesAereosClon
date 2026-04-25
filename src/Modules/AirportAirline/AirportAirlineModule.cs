// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\AirportAirline\AirportAirlineModule.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.AirportAirline.Application.Interfaces;
using GestionAerolineas.src.Modules.AirportAirline.Application.Services;
using GestionAerolineas.src.Modules.AirportAirline.Application.UseCases;
using GestionAerolineas.src.Modules.AirportAirline.Infrastructure.Repository;
using GestionAerolineas.src.Modules.AirportAirline.UI;
using GestionAerolineas.src.Modules.Airlines.Application.UseCases;
using GestionAerolineas.src.Modules.Airlines.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Airports.Application.UseCases;
using GestionAerolineas.src.Modules.Airports.Infrastructure.Repository;
using GestionAerolineas.src.shared.Context;

namespace GestionAerolineas.src.Modules.AirportAirline;

public static class AirportAirlineModule
{
    public static AirportAirlineMenu Build(AppDbContext context)
    {
        var repository = new AirportAirlineRepository(context);

        var airportRepository = new AirportRepository(context);
        var airlineRepository = new AirlineRepository(context);
        IAirportAirlineValidator validator = new AirportAirlineValidator(repository, airportRepository, airlineRepository);

        var create = new CreateAirportAirlineUseCase(repository, validator);
        var getAll = new GetAllAirportAirlinesUseCase(repository);
        var getById = new GetAirportAirlineByIdUseCase(repository);
        var getByPair = new GetAirportAirlineByAirportAndAirlineUseCase(repository);
        var update = new UpdateAirportAirlineUseCase(repository, validator);
        var delete = new DeleteAirportAirlineUseCase(repository);

        var getAllAirports = new GetAllAirportsUseCase(airportRepository);
        var getAllAirlines = new GetAllAirlinesUseCase(airlineRepository);

        return new AirportAirlineMenu(
            create,
            getAll,
            getById,
            getByPair,
            update,
            delete,
            getAllAirports,
            getAllAirlines
        );
    }
}

