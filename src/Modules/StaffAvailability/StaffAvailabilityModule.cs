// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\StaffAvailability\StaffAvailabilityModule.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Airlines.Application.UseCases;
using GestionAerolineas.src.Modules.Airlines.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Airports.Application.UseCases;
using GestionAerolineas.src.Modules.Airports.Infrastructure.Repository;
using GestionAerolineas.src.Modules.AvailabilityStatuses.Application.UseCases;
using GestionAerolineas.src.Modules.AvailabilityStatuses.Infrastructure.Repository;
using GestionAerolineas.src.Modules.People.Application.UseCases;
using GestionAerolineas.src.Modules.People.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Staff.Application.UseCases;
using GestionAerolineas.src.Modules.Staff.Infrastructure.Repository;
using GestionAerolineas.src.Modules.StaffAvailability.Application.Interfaces;
using GestionAerolineas.src.Modules.StaffAvailability.Application.Services;
using GestionAerolineas.src.Modules.StaffAvailability.Application.UseCases;
using GestionAerolineas.src.Modules.StaffAvailability.Infrastructure.Repository;
using GestionAerolineas.src.Modules.StaffAvailability.UI;
using GestionAerolineas.src.Modules.StaffRoles.Application.UseCases;
using GestionAerolineas.src.Modules.StaffRoles.Infrastructure.Repository;
using GestionAerolineas.src.shared.Context;

namespace GestionAerolineas.src.Modules.StaffAvailability;

public static class StaffAvailabilityModule
{
    public static StaffAvailabilityMenu Build(AppDbContext context)
    {
        var repository = new StaffAvailabilityRepository(context);

        var staffRepository = new StaffRepository(context);
        var availabilityStatusRepository = new AvailabilityStatusRepository(context);
        IStaffAvailabilityValidator validator = new StaffAvailabilityValidator(repository, staffRepository, availabilityStatusRepository);

        var create = new CreateStaffAvailabilityUseCase(repository, validator);
        var getAll = new GetAllStaffAvailabilityUseCase(repository);
        var getById = new GetStaffAvailabilityByIdUseCase(repository);
        var getByStaffId = new GetStaffAvailabilityByStaffIdUseCase(repository);
        var getByStatusId = new GetStaffAvailabilityByStatusIdUseCase(repository);
        var getActiveNow = new GetActiveStaffAvailabilityNowByStaffIdUseCase(repository);
        var update = new UpdateStaffAvailabilityUseCase(repository, validator);
        var delete = new DeleteStaffAvailabilityUseCase(repository);

        var personRepository = new PersonRepository(context);
        var staffRoleRepository = new StaffRoleRepository(context);
        var airlineRepository = new AirlineRepository(context);
        var airportRepository = new AirportRepository(context);

        var getAllStaff = new GetAllStaffUseCase(staffRepository);
        var getAllPeople = new GetAllPeopleUseCase(personRepository);
        var getAllStaffRoles = new GetAllStaffRolesUseCase(staffRoleRepository);
        var getAllAirlines = new GetAllAirlinesUseCase(airlineRepository);
        var getAllAirports = new GetAllAirportsUseCase(airportRepository);
        var getAllAvailabilityStatuses = new GetAllAvailabilityStatusesUseCase(availabilityStatusRepository);

        return new StaffAvailabilityMenu(
            create,
            getAll,
            getById,
            getByStaffId,
            getByStatusId,
            getActiveNow,
            update,
            delete,
            getAllStaff,
            getAllPeople,
            getAllStaffRoles,
            getAllAirlines,
            getAllAirports,
            getAllAvailabilityStatuses);
    }
}

