// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Flights\Application\UseCases\UpdateFlightUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Flights.Application.Interfaces;
using GestionAerolineas.src.Modules.Flights.Domain.Aggregate;
using GestionAerolineas.src.Modules.Flights.Domain.Repositories;
using GestionAerolineas.src.Modules.Flights.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Flights.Application.UseCases;

public class UpdateFlightUseCase
{
    private readonly IFlightRepository _repository;
    private readonly IFlightValidator _validator;

    public UpdateFlightUseCase(IFlightRepository repository, IFlightValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(
        int id,
        string code,
        int airlineId,
        int routeId,
        int aircraftId,
        DateTime departureDateTime,
        DateTime estimatedArrivalDateTime,
        int totalCapacity,
        int availableSeats,
        int stateId,
        DateTime? rescheduledAt)
    {
        var idVO = FlightId.Create(id);
        var codeVO = FlightCode.Create(code);
        var airlineIdVO = FlightAirlineId.Create(airlineId);
        var routeIdVO = FlightRouteId.Create(routeId);
        var aircraftIdVO = FlightAircraftId.Create(aircraftId);
        var departureVO = FlightDepartureDateTime.Create(departureDateTime);
        var arrivalVO = FlightEstimatedArrivalDateTime.Create(estimatedArrivalDateTime);
        var totalCapacityVO = FlightTotalCapacity.Create(totalCapacity);
        var availableSeatsVO = FlightAvailableSeats.Create(availableSeats);
        var stateIdVO = FlightStateId.Create(stateId);
        var rescheduledAtVO = FlightRescheduledAt.Create(rescheduledAt);

        await _validator.ValidateUniqueCodeAsync(codeVO, idVO);
        await _validator.ValidateAirlineExistsAsync(airlineIdVO);
        await _validator.ValidateRouteExistsAsync(routeIdVO);
        await _validator.ValidateAircraftExistsAsync(aircraftIdVO);
        await _validator.ValidateStateExistsAsync(stateIdVO);

        _validator.ValidateDateConsistency(departureVO, arrivalVO);
        _validator.ValidateCapacityConsistency(totalCapacityVO, availableSeatsVO);
        _validator.ValidateRescheduledAtConsistency(rescheduledAtVO, departureVO);
        await _validator.ValidateAircraftBelongsToAirlineAsync(aircraftIdVO, airlineIdVO);
        await _validator.ValidateAircraftNoOverlapAsync(aircraftIdVO, departureVO, arrivalVO, idVO);

        var entity = Flight.Create(
            idVO,
            codeVO,
            airlineIdVO,
            routeIdVO,
            aircraftIdVO,
            departureVO,
            arrivalVO,
            totalCapacityVO,
            availableSeatsVO,
            stateIdVO,
            rescheduledAtVO);

        await _repository.UpdateAsync(entity);
    }
}

