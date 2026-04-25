// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationPassengers\Application\UseCases\DeleteReservationPassengerUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Flights.Domain.Aggregate;
using GestionAerolineas.src.Modules.Flights.Domain.ValueObject;
using GestionAerolineas.src.Modules.Flights.Infrastructure.Repository;
using GestionAerolineas.src.Modules.ReservationFlights.Domain.ValueObject;
using GestionAerolineas.src.Modules.ReservationFlights.Infrastructure.Repository;
using GestionAerolineas.src.Modules.ReservationPassengers.Application.Interfaces;
using GestionAerolineas.src.Modules.ReservationPassengers.Domain.Repositories;
using GestionAerolineas.src.Modules.ReservationPassengers.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.ReservationPassengers.Application.UseCases;

public class DeleteReservationPassengerUseCase
{
    private readonly IReservationPassengerRepository _repository;
    private readonly IReservationPassengerValidator _validator;
    private readonly ReservationFlightRepository _reservationFlightRepository;
    private readonly FlightRepository _flightRepository;

    public DeleteReservationPassengerUseCase(
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

    public async Task ExecuteAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(ReservationPassengerId.Create(id));
        if (entity is null)
            return;

        await _validator.ValidateReservationAllowsChangesAsync(entity.ReservationFlightId);

        await _repository.DeleteAsync(entity);
        await ReturnSeatAsync(entity.ReservationFlightId, 1);
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

