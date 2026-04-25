// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Reservations\Application\UseCases\DeleteReservationUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Flights.Domain.Aggregate;
using GestionAerolineas.src.Modules.Flights.Domain.ValueObject;
using GestionAerolineas.src.Modules.Flights.Infrastructure.Repository;
using GestionAerolineas.src.Modules.ReservationFlights.Domain.Repositories;
using GestionAerolineas.src.Modules.ReservationPassengers.Domain.Repositories;
using GestionAerolineas.src.Modules.Reservations.Domain.Repositories;
using GestionAerolineas.src.Modules.Reservations.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Reservations.Application.UseCases;

public class DeleteReservationUseCase
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IReservationFlightRepository _reservationFlightRepository;
    private readonly IReservationPassengerRepository _reservationPassengerRepository;
    private readonly FlightRepository _flightRepository;

    public DeleteReservationUseCase(
        IReservationRepository reservationRepository,
        IReservationFlightRepository reservationFlightRepository,
        IReservationPassengerRepository reservationPassengerRepository,
        FlightRepository flightRepository)
    {
        _reservationRepository = reservationRepository;
        _reservationFlightRepository = reservationFlightRepository;
        _reservationPassengerRepository = reservationPassengerRepository;
        _flightRepository = flightRepository;
    }

    public async Task<bool> ExecuteAsync(int reservationId)
    {
        var reservation = await _reservationRepository.GetByIdAsync(ReservationId.Create(reservationId));
        if (reservation is null)
            return false;

        var reservationFlights = (await _reservationFlightRepository.GetByReservationIdAsync(
            GestionAerolineas.src.Modules.ReservationFlights.Domain.ValueObject.ReservationFlightReservationId.Create(reservationId))).ToList();

        foreach (var rf in reservationFlights)
        {
            var passengers = (await _reservationPassengerRepository.GetByReservationFlightIdAsync(
                GestionAerolineas.src.Modules.ReservationPassengers.Domain.ValueObject.ReservationPassengerReservationFlightId.Create(rf.Id.Value))).ToList();

            if (passengers.Count > 0)
            {
                await ReturnSeatsToFlightAsync(rf.FlightId.Value, passengers.Count);
                foreach (var p in passengers)
                    await _reservationPassengerRepository.DeleteAsync(p);
            }

            await _reservationFlightRepository.DeleteAsync(rf);
        }

        await _reservationRepository.DeleteAsync(reservation);
        return true;
    }

    private async Task ReturnSeatsToFlightAsync(int flightId, int seatsToReturn)
    {
        var flight = await _flightRepository.GetByIdAsync(FlightId.Create(flightId));
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
