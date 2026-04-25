// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Cities\CityModule.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Cities.Application.Interfaces;
using GestionAerolineas.src.Modules.Cities.Application.Services;
using GestionAerolineas.src.Modules.Cities.Application.UseCases;
using GestionAerolineas.src.Modules.Cities.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Cities.UI;
using GestionAerolineas.src.Modules.Regions.Application.UseCases;
using GestionAerolineas.src.Modules.Regions.Infrastructure.Repository;
using GestionAerolineas.src.shared.Context;

namespace GestionAerolineas.src.Modules.Cities;

public static class CityModule
{
    public static CityMenu Build(AppDbContext context)
    {
        var repository = new CityRepository(context);

        var regionRepository = new RegionRepository(context);
        ICityValidator validator = new CityValidator(repository, regionRepository);

        var create = new CreateCityUseCase(repository, validator);
        var getAll = new GetAllCitiesUseCase(repository);
        var getById = new GetCityByIdUseCase(repository);
        var getByName = new GetCityByNameUseCase(repository);
        var update = new UpdateCityUseCase(repository, validator);
        var delete = new DeleteCityUseCase(repository);

        var getAllRegions = new GetAllRegionsUseCase(regionRepository);

        return new CityMenu(create, getAll, getById, getByName, update, delete, getAllRegions);
    }
}
