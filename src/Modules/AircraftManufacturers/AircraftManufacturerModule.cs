// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\AircraftManufacturers\AircraftManufacturerModule.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.AircraftManufacturers.Application.Interfaces;
using GestionAerolineas.src.Modules.AircraftManufacturers.Application.Services;
using GestionAerolineas.src.Modules.AircraftManufacturers.Application.UseCases;
using GestionAerolineas.src.Modules.AircraftManufacturers.Infrastructure.Repository;
using GestionAerolineas.src.Modules.AircraftManufacturers.UI;
using GestionAerolineas.src.Modules.Countries.Application.UseCases;
using GestionAerolineas.src.Modules.Countries.Infrastructure.Repository;
using GestionAerolineas.src.shared.Context;

namespace GestionAerolineas.src.Modules.AircraftManufacturers;

public static class AircraftManufacturerModule
{
    public static AircraftManufacturerMenu Build(AppDbContext context)
    {
        var repository = new AircraftManufacturerRepository(context);
        var countryRepository = new CountryRepository(context);
        IAircraftManufacturerValidator validator = new AircraftManufacturerValidator(repository, countryRepository);

        var create = new CreateAircraftManufacturerUseCase(repository, validator);
        var getAll = new GetAllAircraftManufacturersUseCase(repository);
        var getById = new GetAircraftManufacturerByIdUseCase(repository);
        var getByName = new GetAircraftManufacturerByNameUseCase(repository);
        var update = new UpdateAircraftManufacturerUseCase(repository, validator);
        var delete = new DeleteAircraftManufacturerUseCase(repository);

        var getAllCountries = new GetAllCountriesUseCase(countryRepository);

        return new AircraftManufacturerMenu(
            create,
            getAll,
            getById,
            getByName,
            update,
            delete,
            getAllCountries
        );
    }
}

