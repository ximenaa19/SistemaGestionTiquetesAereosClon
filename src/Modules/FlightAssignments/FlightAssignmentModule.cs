// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightAssignments\FlightAssignmentModule.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Airlines.Application.UseCases;
using GestionAerolineas.src.Modules.Airlines.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Airports.Application.UseCases;
using GestionAerolineas.src.Modules.Airports.Infrastructure.Repository;
using GestionAerolineas.src.Modules.FlightAssignments.Application.Interfaces;
using GestionAerolineas.src.Modules.FlightAssignments.Application.Services;
using GestionAerolineas.src.Modules.FlightAssignments.Application.UseCases;
using GestionAerolineas.src.Modules.FlightAssignments.Infrastructure.Repository;
using GestionAerolineas.src.Modules.FlightAssignments.UI;
using GestionAerolineas.src.Modules.FlightRoles.Application.UseCases;
using GestionAerolineas.src.Modules.FlightRoles.Infrastructure.Repository;
using GestionAerolineas.src.Modules.FlightStates.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Flights.Application.UseCases;
using GestionAerolineas.src.Modules.Flights.Infrastructure.Repository;
using GestionAerolineas.src.Modules.People.Application.UseCases;
using GestionAerolineas.src.Modules.People.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Routes.Application.UseCases;
using GestionAerolineas.src.Modules.Routes.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Staff.Application.UseCases;
using GestionAerolineas.src.Modules.Staff.Infrastructure.Repository;
using GestionAerolineas.src.Modules.StaffRoles.Application.UseCases;
using GestionAerolineas.src.Modules.StaffRoles.Infrastructure.Repository;
using GestionAerolineas.src.shared.Context;

namespace GestionAerolineas.src.Modules.FlightAssignments;

public static class FlightAssignmentModule
{
    public static FlightAssignmentMenu Build(AppDbContext context)
    {
        var repository = new FlightAssignmentRepository(context);

        var flightRepository = new FlightRepository(context);
        var staffRepository = new StaffRepository(context);
        var flightRoleRepository = new FlightRoleRepository(context);
        var routeRepository = new RouteRepository(context);
        var flightStateRepository = new FlightStateRepository(context);

        IFlightAssignmentValidator validator = new FlightAssignmentValidator(
            repository,
            flightRepository,
            staffRepository,
            flightRoleRepository,
            routeRepository,
            flightStateRepository);

        var create = new CreateFlightAssignmentUseCase(repository, validator);
        var getAll = new GetAllFlightAssignmentsUseCase(repository);
        var getById = new GetFlightAssignmentByIdUseCase(repository);
        var getByFlightId = new GetFlightAssignmentsByFlightIdUseCase(repository);
        var getByStaffId = new GetFlightAssignmentsByStaffIdUseCase(repository);
        var getByFlightRoleId = new GetFlightAssignmentsByFlightRoleIdUseCase(repository);
        var getByFlightAndStaff = new GetFlightAssignmentByFlightAndStaffUseCase(repository);
        var update = new UpdateFlightAssignmentUseCase(repository, validator);
        var delete = new DeleteFlightAssignmentUseCase(repository);

        var airportRepository = new AirportRepository(context);
        var airlineRepository = new AirlineRepository(context);
        var personRepository = new PersonRepository(context);
        var staffRoleRepository = new StaffRoleRepository(context);

        var getAllFlights = new GetAllFlightsUseCase(flightRepository);
        var getAllRoutes = new GetAllRoutesUseCase(routeRepository);
        var getAllAirports = new GetAllAirportsUseCase(airportRepository);
        var getAllAirlines = new GetAllAirlinesUseCase(airlineRepository);
        var getAllStaff = new GetAllStaffUseCase(staffRepository);
        var getAllPeople = new GetAllPeopleUseCase(personRepository);
        var getAllStaffRoles = new GetAllStaffRolesUseCase(staffRoleRepository);
        var getAllFlightRoles = new GetAllFlightRolesUseCase(flightRoleRepository);

        return new FlightAssignmentMenu(
            create,
            getAll,
            getById,
            getByFlightId,
            getByStaffId,
            getByFlightRoleId,
            getByFlightAndStaff,
            update,
            delete,
            getAllFlights,
            getAllRoutes,
            getAllAirports,
            getAllAirlines,
            getAllStaff,
            getAllPeople,
            getAllStaffRoles,
            getAllFlightRoles);
    }
}

