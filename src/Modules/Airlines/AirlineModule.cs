// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Airlines\AirlineModule.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Airlines.Application.Interfaces;
using GestionAerolineas.src.Modules.Airlines.Application.Services;
using GestionAerolineas.src.Modules.Airlines.Application.UseCases;
using GestionAerolineas.src.Modules.Airlines.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Airlines.UI;
using GestionAerolineas.src.Modules.Countries.Application.UseCases;
using GestionAerolineas.src.Modules.Countries.Infrastructure.Repository;
using GestionAerolineas.src.shared.Context;

namespace GestionAerolineas.src.Modules.Airlines;

public static class AirlineModule
{
    public static AirlineMenu Build(AppDbContext context)
    {
        var repository = new AirlineRepository(context);

        var countryRepository = new CountryRepository(context);
        IAirlineValidator validator = new AirlineValidator(repository, countryRepository);

        var create = new CreateAirlineUseCase(repository, validator);
        var getAll = new GetAllAirlinesUseCase(repository);
        var getById = new GetAirlineByIdUseCase(repository);
        var getByName = new GetAirlineByNameUseCase(repository);
        var update = new UpdateAirlineUseCase(repository, validator);
        var delete = new DeleteAirlineUseCase(repository);

        var getAllCountries = new GetAllCountriesUseCase(countryRepository);

        return new AirlineMenu(create, getAll, getById, getByName, update, delete, getAllCountries);
    }

    public static AdminCreateAirlineFlow BuildAdminCreateFlow(AppDbContext context)
    {
        var repository = new AirlineRepository(context);
        var countryRepository = new CountryRepository(context);

        IAirlineValidator validator = new AirlineValidator(repository, countryRepository);

        var create = new CreateAirlineUseCase(repository, validator);
        var getAllCountries = new GetAllCountriesUseCase(countryRepository);

        return new AdminCreateAirlineFlow(create, getAllCountries);
    }

    public static AdminUpdateAirlineFlow BuildAdminUpdateFlow(AppDbContext context)
    {
        var repository = new AirlineRepository(context);
        var countryRepository = new CountryRepository(context);

        IAirlineValidator validator = new AirlineValidator(repository, countryRepository);

        var getAll = new GetAllAirlinesUseCase(repository);
        var getById = new GetAirlineByIdUseCase(repository);
        var update = new UpdateAirlineUseCase(repository, validator);
        var getAllCountries = new GetAllCountriesUseCase(countryRepository);

        return new AdminUpdateAirlineFlow(getAll, getById, update, getAllCountries);
    }

    public static AdminDeleteAirlineFlow BuildAdminDeleteFlow(AppDbContext context)
    {
        var repository = new AirlineRepository(context);

        var getAll = new GetAllAirlinesUseCase(repository);
        var getById = new GetAirlineByIdUseCase(repository);
        var delete = new DeleteAirlineUseCase(repository);

        return new AdminDeleteAirlineFlow(getAll, getById, delete);
    }
}

