// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationPassengers\Application\Services\ReservationPassengerValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Passengers.Domain.ValueObject;
using GestionAerolineas.src.Modules.Passengers.Infrastructure.Repository;
using GestionAerolineas.src.Modules.ReservationFlights.Domain.ValueObject;
using GestionAerolineas.src.Modules.ReservationFlights.Infrastructure.Repository;
using GestionAerolineas.src.Modules.ReservationPassengers.Application.Interfaces;
using GestionAerolineas.src.Modules.ReservationPassengers.Domain.Repositories;
using GestionAerolineas.src.Modules.ReservationPassengers.Domain.ValueObject;
using GestionAerolineas.src.Modules.ReservationStatuses.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Reservations.Domain.ValueObject;
using GestionAerolineas.src.Modules.Reservations.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Flights.Domain.ValueObject;
using GestionAerolineas.src.Modules.Flights.Infrastructure.Repository;

namespace GestionAerolineas.src.Modules.ReservationPassengers.Application.Services;

public class ReservationPassengerValidator : IReservationPassengerValidator
{
    private readonly IReservationPassengerRepository _repository;
    private readonly ReservationFlightRepository _reservationFlightRepository;
    private readonly PassengerRepository _passengerRepository;
    private readonly ReservationRepository _reservationRepository;
    private readonly ReservationStatusRepository _reservationStatusRepository;
    private readonly FlightRepository _flightRepository;

    public ReservationPassengerValidator(
        IReservationPassengerRepository repository,
        ReservationFlightRepository reservationFlightRepository,
        PassengerRepository passengerRepository,
        ReservationRepository reservationRepository,
        ReservationStatusRepository reservationStatusRepository,
        FlightRepository flightRepository)
    {
        _repository = repository;
        _reservationFlightRepository = reservationFlightRepository;
        _passengerRepository = passengerRepository;
        _reservationRepository = reservationRepository;
        _reservationStatusRepository = reservationStatusRepository;
        _flightRepository = flightRepository;
    }

    public async Task ValidateReservationFlightExistsAsync(ReservationPassengerReservationFlightId reservationFlightId)
    {
        var exists = await _reservationFlightRepository.ExistsAsync(ReservationFlightId.Create(reservationFlightId.Value));
        if (!exists)
            throw new Exception("El reserva_vuelo_id no existe");
    }

    public async Task ValidatePassengerExistsAsync(ReservationPassengerPassengerId passengerId)
    {
        var exists = await _passengerRepository.ExistsAsync(PassengerId.Create(passengerId.Value));
        if (!exists)
            throw new Exception("El pasajero no existe");
    }

    public async Task ValidateUniquePairAsync(
        ReservationPassengerReservationFlightId reservationFlightId,
        ReservationPassengerPassengerId passengerId,
        ReservationPassengerId? currentId = null)
    {
        var exists = await _repository.ExistsByReservationFlightAndPassengerAsync(
            reservationFlightId.Value,
            passengerId.Value,
            currentId?.Value);

        if (exists)
            throw new Exception("Ese pasajero ya esta agregado a ese reserva_vuelo");
    }

    public async Task ValidateReservationAllowsChangesAsync(ReservationPassengerReservationFlightId reservationFlightId)
    {
        var reservationFlight = await _reservationFlightRepository.GetByIdAsync(ReservationFlightId.Create(reservationFlightId.Value));
        if (reservationFlight is null)
            throw new Exception("El reserva_vuelo_id no existe");

        var reservation = await _reservationRepository.GetByIdAsync(ReservationId.Create(reservationFlight.ReservationId.Value));
        if (reservation is null)
            throw new Exception("La reserva no existe");

        var status = await _reservationStatusRepository.GetByIdAsync(
            GestionAerolineas.src.Modules.ReservationStatuses.Domain.ValueObject.ReservationStatusId.Create(reservation.StatusId.Value));

        var name = (status?.Name.Value ?? string.Empty).Trim().ToUpperInvariant();
        if (name == "CANCELADA" || name == "VENCIDA")
            throw new Exception($"No se pueden modificar pasajeros en una reserva '{status!.Name.Value}'");
    }

    public async Task ValidateFlightHasAvailabilityAsync(ReservationPassengerReservationFlightId reservationFlightId, int seatsToConsume)
    {
        if (seatsToConsume <= 0)
            return;

        var reservationFlight = await _reservationFlightRepository.GetByIdAsync(ReservationFlightId.Create(reservationFlightId.Value));
        if (reservationFlight is null)
            throw new Exception("El reserva_vuelo_id no existe");

        var flight = await _flightRepository.GetByIdAsync(FlightId.Create(reservationFlight.FlightId.Value));
        if (flight is null)
            throw new Exception("El vuelo no existe");

        if (flight.AvailableSeats.Value < seatsToConsume)
            throw new Exception("No hay asientos suficientes disponibles en el vuelo");
    }
}

