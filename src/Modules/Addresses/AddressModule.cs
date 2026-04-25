// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Addresses\AddressModule.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Addresses.Application.Interfaces;
using GestionAerolineas.src.Modules.Addresses.Application.Services;
using GestionAerolineas.src.Modules.Addresses.Application.UseCases;
using GestionAerolineas.src.Modules.Addresses.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Addresses.UI;
using GestionAerolineas.src.Modules.Cities.Application.UseCases;
using GestionAerolineas.src.Modules.Cities.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Regions.Application.UseCases;
using GestionAerolineas.src.Modules.Regions.Infrastructure.Repository;
using GestionAerolineas.src.Modules.RoadTypes.Application.UseCases;
using GestionAerolineas.src.Modules.RoadTypes.Infrastructure.Repository;
using GestionAerolineas.src.shared.Context;

namespace GestionAerolineas.src.Modules.Addresses;

public static class AddressModule
{
    public static AddressMenu Build(AppDbContext context)
    {
        var repository = new AddressRepository(context);

        var roadTypeRepository = new RoadTypeRepository(context);
        var cityRepository = new CityRepository(context);
        IAddressValidator validator = new AddressValidator(roadTypeRepository, cityRepository);

        var create = new CreateAddressUseCase(repository, validator);
        var getAll = new GetAllAddressesUseCase(repository);
        var getById = new GetAddressByIdUseCase(repository);
        var update = new UpdateAddressUseCase(repository, validator);
        var delete = new DeleteAddressUseCase(repository);

        var getAllRoadTypes = new GetAllRoadTypesUseCase(roadTypeRepository);
        var getAllCities = new GetAllCitiesUseCase(cityRepository);

        var regionRepository = new RegionRepository(context);
        var getAllRegions = new GetAllRegionsUseCase(regionRepository);

        return new AddressMenu(
            create,
            getAll,
            getById,
            update,
            delete,
            getAllRoadTypes,
            getAllCities,
            getAllRegions
        );
    }
}

