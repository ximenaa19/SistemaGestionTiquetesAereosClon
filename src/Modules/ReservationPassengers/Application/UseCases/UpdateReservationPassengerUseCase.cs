// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationPassengers\Application\UseCases\UpdateReservationPassengerUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Flights.Domain.Aggregate;
using GestionAerolineas.src.Modules.Flights.Domain.ValueObject;
using GestionAerolineas.src.Modules.Flights.Infrastructure.Repository;
using GestionAerolineas.src.Modules.ReservationFlights.Domain.ValueObject;
using GestionAerolineas.src.Modules.ReservationFlights.Infrastructure.Repository;
using GestionAerolineas.src.Modules.ReservationPassengers.Application.Interfaces;
using GestionAerolineas.src.Modules.ReservationPassengers.Domain.Aggregate;
using GestionAerolineas.src.Modules.ReservationPassengers.Domain.Repositories;
using GestionAerolineas.src.Modules.ReservationPassengers.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.ReservationPassengers.Application.UseCases;

public class UpdateReservationPassengerUseCase
{
    private readonly IReservationPassengerRepository _repository;
    private readonly IReservationPassengerValidator _validator;
    private readonly ReservationFlightRepository _reservationFlightRepository;
    private readonly FlightRepository _flightRepository;

    public UpdateReservationPassengerUseCase(
        IReservationPassengerRepository repository,
        IReservationPassengerValidator validator,
        ReservationFlightRepository reservationFlightRepository,
        FlightRepository flightRepository)
    {
        _repository = repository;
        _validator = validator;
        _reservationFlightRepository = reservationFlightRepository;
        _flightRepository = flightRepository;
    }

    public async Task ExecuteAsync(int id, int reservationFlightId, int passengerId)
    {
        var idVO = ReservationPassengerId.Create(id);
        var newReservationFlightIdVO = ReservationPassengerReservationFlightId.Create(reservationFlightId);
        var newPassengerIdVO = ReservationPassengerPassengerId.Create(passengerId);

        var existing = await _repository.GetByIdAsync(idVO);
        if (existing is null)
            return;

        await _validator.ValidateReservationFlightExistsAsync(newReservationFlightIdVO);
        await _validator.ValidatePassengerExistsAsync(newPassengerIdVO);
        await _validator.ValidateReservationAllowsChangesAsync(newReservationFlightIdVO);
        await _validator.ValidateUniquePairAsync(newReservationFlightIdVO, newPassengerIdVO, idVO);

        // If flight changes, we need to return seat to old flight and consume on new flight.
        var oldReservationFlight = await _reservationFlightRepository.GetByIdAsync(ReservationFlightId.Create(existing.ReservationFlightId.Value));
        var newReservationFlight = await _reservationFlightRepository.GetByIdAsync(ReservationFlightId.Create(newReservationFlightIdVO.Value));

        if (newReservationFlight is null)
            throw new Exception("El reserva_vuelo_id no existe");

        if (oldReservationFlight is null || oldReservationFlight.FlightId.Value != newReservationFlight.FlightId.Value)
        {
            await _validator.ValidateFlightHasAvailabilityAsync(newReservationFlightIdVO, 1);
            await ReturnSeatAsync(existing.ReservationFlightId, 1);
            await ConsumeSeatAsync(newReservationFlightIdVO, 1);
        }

        var updated = ReservationPassenger.Create(idVO, newReservationFlightIdVO, newPassengerIdVO);
        await _repository.UpdateAsync(updated);
    }

    private async Task ConsumeSeatAsync(ReservationPassengerReservationFlightId reservationFlightId, int seatsToConsume)
    {
        var reservationFlight = await _reservationFlightRepository.GetByIdAsync(ReservationFlightId.Create(reservationFlightId.Value));
        if (reservationFlight is null)
            return;

        var flight = await _flightRepository.GetByIdAsync(FlightId.Create(reservationFlight.FlightId.Value));
        if (flight is null)
            return;

        var updatedSeats = FlightAvailableSeats.Create(flight.AvailableSeats.Value - seatsToConsume);
        var updated = Flight.Create(
            flight.Id,
            flight.Code,
            flight.AirlineId,
            flight.RouteId,
            flight.AircraftId,
            flight.DepartureDateTime,
            flight.EstimatedArrivalDateTime,
            flight.TotalCapacity,
            updatedSeats,
            flight.StateId,
            flight.RescheduledAt);

        await _flightRepository.UpdateAsync(updated);
    }

    private async Task ReturnSeatAsync(ReservationPassengerReservationFlightId reservationFlightId, int seatsToReturn)
    {
        var reservationFlight = await _reservationFlightRepository.GetByIdAsync(ReservationFlightId.Create(reservationFlightId.Value));
        if (reservationFlight is null)
            return;

        var flight = await _flightRepository.GetByIdAsync(FlightId.Create(reservationFlight.FlightId.Value));
        if (flight is null)
            return;

        var newValue = Math.Min(flight.TotalCapacity.Value, flight.AvailableSeats.Value + seatsToReturn);
        var updatedSeats = FlightAvailableSeats.Create(newValue);
        var updated = Flight.Create(
            flight.Id,
            flight.Code,
            flight.AirlineId,
            flight.RouteId,
            flight.AircraftId,
            flight.DepartureDateTime,
            flight.EstimatedArrivalDateTime,
            flight.TotalCapacity,
            updatedSeats,
            flight.StateId,
            flight.RescheduledAt);

        await _flightRepository.UpdateAsync(updated);
    }
}

