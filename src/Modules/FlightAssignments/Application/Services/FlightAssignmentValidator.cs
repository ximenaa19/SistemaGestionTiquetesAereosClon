// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightAssignments\Application\Services\FlightAssignmentValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.FlightAssignments.Application.Interfaces;
using GestionAerolineas.src.Modules.FlightAssignments.Domain.Repositories;
using GestionAerolineas.src.Modules.FlightAssignments.Domain.ValueObject;
using GestionAerolineas.src.Modules.FlightRoles.Domain.ValueObject;
using GestionAerolineas.src.Modules.FlightRoles.Infrastructure.Repository;
using FlightStatesFlightStateId = GestionAerolineas.src.Modules.FlightStates.Domain.ValueObject.FlightStateId;
using GestionAerolineas.src.Modules.FlightStates.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Flights.Domain.ValueObject;
using GestionAerolineas.src.Modules.Flights.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Routes.Domain.ValueObject;
using GestionAerolineas.src.Modules.Routes.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Staff.Domain.ValueObject;
using GestionAerolineas.src.Modules.Staff.Infrastructure.Repository;

namespace GestionAerolineas.src.Modules.FlightAssignments.Application.Services;

public class FlightAssignmentValidator : IFlightAssignmentValidator
{
    private readonly IFlightAssignmentRepository _repository;
    private readonly FlightRepository _flightRepository;
    private readonly StaffRepository _staffRepository;
    private readonly FlightRoleRepository _flightRoleRepository;
    private readonly RouteRepository _routeRepository;
    private readonly FlightStateRepository _flightStateRepository;

    public FlightAssignmentValidator(
        IFlightAssignmentRepository repository,
        FlightRepository flightRepository,
        StaffRepository staffRepository,
        FlightRoleRepository flightRoleRepository,
        RouteRepository routeRepository,
        FlightStateRepository flightStateRepository)
    {
        _repository = repository;
        _flightRepository = flightRepository;
        _staffRepository = staffRepository;
        _flightRoleRepository = flightRoleRepository;
        _routeRepository = routeRepository;
        _flightStateRepository = flightStateRepository;
    }

    public async Task ValidateFlightExistsAsync(FlightAssignmentFlightId flightId)
    {
        var exists = await _flightRepository.ExistsAsync(FlightId.Create(flightId.Value));
        if (!exists)
            throw new Exception("El vuelo no existe");
    }

    public async Task ValidateStaffExistsAndActiveAsync(FlightAssignmentStaffId staffId)
    {
        var staff = await _staffRepository.GetByIdAsync(StaffId.Create(staffId.Value));
        if (staff is null)
            throw new Exception("El staff no existe");

        if (!staff.IsActive.Value)
            throw new Exception("No se puede asignar staff inactivo a un vuelo");
    }

    public async Task ValidateFlightRoleExistsAsync(FlightAssignmentFlightRoleId flightRoleId)
    {
        var exists = await _flightRoleRepository.ExistsAsync(FlightRoleId.Create(flightRoleId.Value));
        if (!exists)
            throw new Exception("El rol de vuelo no existe");
    }

    public async Task ValidateUniqueFlightAndStaffAsync(FlightAssignmentFlightId flightId, FlightAssignmentStaffId staffId, FlightAssignmentId? currentId = null)
    {
        var exists = await _repository.ExistsByFlightAndStaffAsync(flightId.Value, staffId.Value, currentId?.Value);
        if (exists)
            throw new Exception("Ese staff ya esta asignado a ese vuelo");
    }

    public async Task ValidateNoStaffOverlapAsync(FlightAssignmentStaffId staffId, FlightAssignmentFlightId flightId, FlightAssignmentId? currentId = null)
    {
        var flight = await _flightRepository.GetByIdAsync(FlightId.Create(flightId.Value));
        if (flight is null)
            throw new Exception("El vuelo no existe");

        var overlap = await _repository.ExistsStaffOverlapAsync(staffId.Value, flight.DepartureDateTime.Value, flight.EstimatedArrivalDateTime.Value, currentId?.Value);
        if (overlap)
            throw new Exception("El staff ya tiene otro vuelo asignado que se solapa en ese rango de tiempo");
    }

    public async Task ValidateStaffAirlineConsistencyAsync(FlightAssignmentStaffId staffId, FlightAssignmentFlightId flightId)
    {
        var staff = await _staffRepository.GetByIdAsync(StaffId.Create(staffId.Value));
        if (staff is null)
            throw new Exception("El staff no existe");

        var flight = await _flightRepository.GetByIdAsync(FlightId.Create(flightId.Value));
        if (flight is null)
            throw new Exception("El vuelo no existe");

        if (staff.AirlineId.Value.HasValue && staff.AirlineId.Value.Value != flight.AirlineId.Value)
            throw new Exception("El staff pertenece a otra aerolinea");
    }

    public async Task ValidateAirportStaffMatchesRouteAsync(FlightAssignmentStaffId staffId, FlightAssignmentFlightId flightId)
    {
        var staff = await _staffRepository.GetByIdAsync(StaffId.Create(staffId.Value));
        if (staff is null)
            throw new Exception("El staff no existe");

        if (!staff.AirportId.Value.HasValue)
            return;

        var flight = await _flightRepository.GetByIdAsync(FlightId.Create(flightId.Value));
        if (flight is null)
            throw new Exception("El vuelo no existe");

        var route = await _routeRepository.GetByIdAsync(RouteId.Create(flight.RouteId.Value));
        if (route is null)
            throw new Exception("La ruta no existe");

        var airportId = staff.AirportId.Value.Value;
        if (route.OriginAirportId.Value != airportId && route.DestinationAirportId.Value != airportId)
            throw new Exception("El staff de aeropuerto debe pertenecer al aeropuerto de origen o destino de la ruta");
    }

    public async Task ValidateFlightNotInFinalStateAsync(FlightAssignmentFlightId flightId)
    {
        var flight = await _flightRepository.GetByIdAsync(FlightId.Create(flightId.Value));
        if (flight is null)
            throw new Exception("El vuelo no existe");

        var state = await _flightStateRepository.GetByIdAsync(FlightStatesFlightStateId.Create(flight.StateId.Value));
        var name = (state?.Name.Value ?? string.Empty).Trim().ToUpperInvariant();

        if (name == "CANCELADO" || name == "COMPLETADO")
            throw new Exception($"No se pueden asignar roles a un vuelo en estado '{state!.Name.Value}'");
    }
}
