// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Airports\AirportModule.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Airports.Application.Interfaces;
using GestionAerolineas.src.Modules.Airports.Application.Services;
using GestionAerolineas.src.Modules.Airports.Application.UseCases;
using GestionAerolineas.src.Modules.Airports.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Airports.UI;
using GestionAerolineas.src.Modules.Cities.Application.UseCases;
using GestionAerolineas.src.Modules.Cities.Infrastructure.Repository;
using GestionAerolineas.src.shared.Context;

namespace GestionAerolineas.src.Modules.Airports;

public static class AirportModule
{
    public static AirportMenu Build(AppDbContext context)
    {
        var repository = new AirportRepository(context);

        var cityRepository = new CityRepository(context);
        IAirportValidator validator = new AirportValidator(repository, cityRepository);

        var create = new CreateAirportUseCase(repository, validator);
        var getAll = new GetAllAirportsUseCase(repository);
        var getById = new GetAirportByIdUseCase(repository);
        var getByName = new GetAirportByNameUseCase(repository);
        var update = new UpdateAirportUseCase(repository, validator);
        var delete = new DeleteAirportUseCase(repository);

        var getAllCities = new GetAllCitiesUseCase(cityRepository);

        return new AirportMenu(create, getAll, getById, getByName, update, delete, getAllCities);
    }

    public static AdminCreateAirportFlow BuildAdminCreateFlow(AppDbContext context)
    {
        var repository = new AirportRepository(context);
        var cityRepository = new CityRepository(context);

        IAirportValidator validator = new AirportValidator(repository, cityRepository);

        var create = new CreateAirportUseCase(repository, validator);
        var getAllCities = new GetAllCitiesUseCase(cityRepository);

        return new AdminCreateAirportFlow(create, getAllCities);
    }

    public static AdminUpdateAirportFlow BuildAdminUpdateFlow(AppDbContext context)
    {
        var repository = new AirportRepository(context);
        var cityRepository = new CityRepository(context);

        IAirportValidator validator = new AirportValidator(repository, cityRepository);

        var getAll = new GetAllAirportsUseCase(repository);
        var getById = new GetAirportByIdUseCase(repository);
        var update = new UpdateAirportUseCase(repository, validator);
        var getAllCities = new GetAllCitiesUseCase(cityRepository);

        return new AdminUpdateAirportFlow(getAll, getById, update, getAllCities);
    }

    public static AdminDeleteAirportFlow BuildAdminDeleteFlow(AppDbContext context)
    {
        var repository = new AirportRepository(context);

        var getAll = new GetAllAirportsUseCase(repository);
        var getById = new GetAirportByIdUseCase(repository);
        var delete = new DeleteAirportUseCase(repository);

        return new AdminDeleteAirportFlow(getAll, getById, delete);
    }
}
