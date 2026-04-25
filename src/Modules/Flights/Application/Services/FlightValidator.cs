// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Flights\Application\Services\FlightValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Aircraft.Domain.ValueObject;
using GestionAerolineas.src.Modules.Aircraft.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Airlines.Domain.ValueObject;
using GestionAerolineas.src.Modules.Airlines.Infrastructure.Repository;
using GestionAerolineas.src.Modules.FlightStates.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Flights.Application.Interfaces;
using GestionAerolineas.src.Modules.Flights.Domain.Repositories;
using GestionAerolineas.src.Modules.Flights.Domain.ValueObject;
using GestionAerolineas.src.Modules.Routes.Domain.ValueObject;
using GestionAerolineas.src.Modules.Routes.Infrastructure.Repository;
using FlightStatesFlightStateId = GestionAerolineas.src.Modules.FlightStates.Domain.ValueObject.FlightStateId;

namespace GestionAerolineas.src.Modules.Flights.Application.Services;

public class FlightValidator : IFlightValidator
{
    private readonly IFlightRepository _repository;
    private readonly AirlineRepository _airlineRepository;
    private readonly RouteRepository _routeRepository;
    private readonly AircraftRepository _aircraftRepository;
    private readonly FlightStateRepository _flightStateRepository;

    public FlightValidator(
        IFlightRepository repository,
        AirlineRepository airlineRepository,
        RouteRepository routeRepository,
        AircraftRepository aircraftRepository,
        FlightStateRepository flightStateRepository)
    {
        _repository = repository;
        _airlineRepository = airlineRepository;
        _routeRepository = routeRepository;
        _aircraftRepository = aircraftRepository;
        _flightStateRepository = flightStateRepository;
    }

    public async Task ValidateAirlineExistsAsync(FlightAirlineId airlineId)
    {
        var exists = await _airlineRepository.ExistsAsync(AirlineId.Create(airlineId.Value));
        if (!exists)
            throw new Exception("La aerolinea no existe");
    }

    public async Task ValidateRouteExistsAsync(FlightRouteId routeId)
    {
        var exists = await _routeRepository.ExistsAsync(RouteId.Create(routeId.Value));
        if (!exists)
            throw new Exception("La ruta no existe");
    }

    public async Task ValidateAircraftExistsAsync(FlightAircraftId aircraftId)
    {
        var exists = await _aircraftRepository.ExistsAsync(AircraftId.Create(aircraftId.Value));
        if (!exists)
            throw new Exception("La aeronave no existe");
    }

    public async Task ValidateStateExistsAsync(FlightStateId stateId)
    {
        var exists = await _flightStateRepository.ExistsAsync(FlightStatesFlightStateId.Create(stateId.Value));
        if (!exists)
            throw new Exception("El estado de vuelo no existe");
    }

    public async Task ValidateAircraftBelongsToAirlineAsync(FlightAircraftId aircraftId, FlightAirlineId airlineId)
    {
        var aircraft = await _aircraftRepository.GetByIdAsync(AircraftId.Create(aircraftId.Value));
        if (aircraft is null)
            throw new Exception("La aeronave no existe");

        if (aircraft.AirlineId.Value != airlineId.Value)
            throw new Exception("La aeronave pertenece a otra aerolinea");
    }

    public async Task ValidateUniqueCodeAsync(FlightCode code, FlightId? currentId = null)
    {
        var normalized = FlightCode.Normalize(code.Value);
        var exists = await _repository.ExistsByNormalizedCodeAsync(normalized, currentId?.Value);
        if (exists)
            throw new Exception("Ya existe un vuelo con ese codigo_vuelo");
    }

    public void ValidateDateConsistency(FlightDepartureDateTime departure, FlightEstimatedArrivalDateTime estimatedArrival)
    {
        if (estimatedArrival.Value <= departure.Value)
            throw new Exception("La fecha_llegada_estimada debe ser mayor que la fecha_salida");
    }

    public void ValidateCapacityConsistency(FlightTotalCapacity totalCapacity, FlightAvailableSeats availableSeats)
    {
        if (availableSeats.Value > totalCapacity.Value)
            throw new Exception("Los asientos_disponibles no pueden ser mayores que la capacidad_total");
    }

    public void ValidateRescheduledAtConsistency(FlightRescheduledAt rescheduledAt, FlightDepartureDateTime departure)
    {
        if (rescheduledAt.Value.HasValue && rescheduledAt.Value.Value < departure.Value)
            throw new Exception("reprogramado_en no puede ser menor que fecha_salida");
    }

    public async Task ValidateAircraftNoOverlapAsync(
        FlightAircraftId aircraftId,
        FlightDepartureDateTime departure,
        FlightEstimatedArrivalDateTime estimatedArrival,
        FlightId? currentId = null)
    {
        var overlap = await _repository.ExistsAircraftOverlapAsync(aircraftId.Value, departure.Value, estimatedArrival.Value, currentId?.Value);
        if (overlap)
            throw new Exception("La aeronave ya tiene otro vuelo que se solapa en ese rango de tiempo");
    }
}
