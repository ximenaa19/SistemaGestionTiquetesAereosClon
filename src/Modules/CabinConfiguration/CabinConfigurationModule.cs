// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\CabinConfiguration\CabinConfigurationModule.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Aircraft.Application.UseCases;
using GestionAerolineas.src.Modules.Aircraft.Infrastructure.Repository;
using GestionAerolineas.src.Modules.CabinConfiguration.Application.Interfaces;
using GestionAerolineas.src.Modules.CabinConfiguration.Application.Services;
using GestionAerolineas.src.Modules.CabinConfiguration.Application.UseCases;
using GestionAerolineas.src.Modules.CabinConfiguration.Infrastructure.Repository;
using GestionAerolineas.src.Modules.CabinConfiguration.UI;
using GestionAerolineas.src.Modules.CabinTypes.Application.UseCases;
using GestionAerolineas.src.Modules.CabinTypes.Infrastructure.Repository;
using GestionAerolineas.src.shared.Context;

namespace GestionAerolineas.src.Modules.CabinConfiguration;

public static class CabinConfigurationModule
{
    public static CabinConfigurationMenu Build(AppDbContext context)
    {
        var repository = new CabinConfigurationRepository(context);

        var aircraftRepository = new AircraftRepository(context);
        var cabinTypeRepository = new CabinTypeRepository(context);

        ICabinConfigurationValidator validator = new CabinConfigurationValidator(repository, aircraftRepository, cabinTypeRepository);

        var create = new CreateCabinConfigurationUseCase(repository, validator);
        var getAll = new GetAllCabinConfigurationsUseCase(repository);
        var getById = new GetCabinConfigurationByIdUseCase(repository);
        var getByAircraftId = new GetCabinConfigurationsByAircraftIdUseCase(repository);
        var getByAircraftAndCabinType = new GetCabinConfigurationByAircraftAndCabinTypeUseCase(repository);
        var update = new UpdateCabinConfigurationUseCase(repository, validator);
        var delete = new DeleteCabinConfigurationUseCase(repository);

        var getAllAircraft = new GetAllAircraftUseCase(aircraftRepository);
        var getAllCabinTypes = new GetAllCabinTypeUseCase(cabinTypeRepository);

        return new CabinConfigurationMenu(
            create,
            getAll,
            getById,
            getByAircraftId,
            getByAircraftAndCabinType,
            update,
            delete,
            getAllAircraft,
            getAllCabinTypes
        );
    }
}

