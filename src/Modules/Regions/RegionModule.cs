// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Regions\RegionModule.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Countries.Application.UseCases;
using GestionAerolineas.src.Modules.Countries.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Regions.Application.Interfaces;
using GestionAerolineas.src.Modules.Regions.Application.Services;
using GestionAerolineas.src.Modules.Regions.Application.UseCases;
using GestionAerolineas.src.Modules.Regions.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Regions.UI;
using GestionAerolineas.src.shared.Context;

namespace GestionAerolineas.src.Modules.Regions;

public static class RegionModule
{
    public static RegionMenu Build(AppDbContext context)
    {
        var repository = new RegionRepository(context);

        var countryRepository = new CountryRepository(context);
        IRegionValidator validator = new RegionValidator(repository, countryRepository);

        var create = new CreateRegionUseCase(repository, validator);
        var getAll = new GetAllRegionsUseCase(repository);
        var getById = new GetRegionByIdUseCase(repository);
        var getByName = new GetRegionByNameUseCase(repository);
        var update = new UpdateRegionUseCase(repository, validator);
        var delete = new DeleteRegionUseCase(repository);

        var getAllCountries = new GetAllCountriesUseCase(countryRepository);

        return new RegionMenu(create, getAll, getById, getByName, update, delete, getAllCountries);
    }
}

