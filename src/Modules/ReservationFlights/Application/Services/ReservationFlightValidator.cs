// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationFlights\Application\Services\ReservationFlightValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.FlightStates.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Flights.Domain.ValueObject;
using GestionAerolineas.src.Modules.Flights.Infrastructure.Repository;
using GestionAerolineas.src.Modules.ReservationFlights.Application.Interfaces;
using GestionAerolineas.src.Modules.ReservationFlights.Domain.Repositories;
using GestionAerolineas.src.Modules.ReservationFlights.Domain.ValueObject;
using GestionAerolineas.src.Modules.Reservations.Domain.ValueObject;
using GestionAerolineas.src.Modules.Reservations.Infrastructure.Repository;
using GestionAerolineas.src.Modules.ReservationStatuses.Infrastructure.Repository;
using GestionAerolineas.src.Modules.ReservationPassengers.Infrastructure.Repository;
using GestionAerolineas.src.Modules.ReservationPassengers.Domain.ValueObject;
using FlightStatesFlightStateId = GestionAerolineas.src.Modules.FlightStates.Domain.ValueObject.FlightStateId;

namespace GestionAerolineas.src.Modules.ReservationFlights.Application.Services;

public class ReservationFlightValidator : IReservationFlightValidator
{
    private readonly IReservationFlightRepository _repository;
    private readonly ReservationRepository _reservationRepository;
    private readonly FlightRepository _flightRepository;
    private readonly FlightStateRepository _flightStateRepository;
    private readonly ReservationStatusRepository _reservationStatusRepository;
    private readonly ReservationPassengerRepository _reservationPassengerRepository;

    public ReservationFlightValidator(
        IReservationFlightRepository repository,
        ReservationRepository reservationRepository,
        FlightRepository flightRepository,
        FlightStateRepository flightStateRepository,
        ReservationStatusRepository reservationStatusRepository,
        ReservationPassengerRepository reservationPassengerRepository)
    {
        _repository = repository;
        _reservationRepository = reservationRepository;
        _flightRepository = flightRepository;
        _flightStateRepository = flightStateRepository;
        _reservationStatusRepository = reservationStatusRepository;
        _reservationPassengerRepository = reservationPassengerRepository;
    }

    public async Task ValidateReservationExistsAsync(ReservationFlightReservationId reservationId)
    {
        var exists = await _reservationRepository.ExistsAsync(ReservationId.Create(reservationId.Value));
        if (!exists)
            throw new Exception("La reserva no existe");
    }

    public async Task ValidateFlightExistsAsync(ReservationFlightFlightId flightId)
    {
        var exists = await _flightRepository.ExistsAsync(FlightId.Create(flightId.Value));
        if (!exists)
            throw new Exception("El vuelo no existe");
    }

    public async Task ValidateUniquePairAsync(ReservationFlightReservationId reservationId, ReservationFlightFlightId flightId, ReservationFlightId? currentId = null)
    {
        var exists = await _repository.ExistsByReservationAndFlightAsync(reservationId.Value, flightId.Value, currentId?.Value);
        if (exists)
            throw new Exception("Ese vuelo ya esta agregado a la reserva");
    }

    public async Task ValidateFlightNotInFinalStateAsync(ReservationFlightFlightId flightId)
    {
        var flight = await _flightRepository.GetByIdAsync(FlightId.Create(flightId.Value));
        if (flight is null)
            throw new Exception("El vuelo no existe");

        var state = await _flightStateRepository.GetByIdAsync(FlightStatesFlightStateId.Create(flight.StateId.Value));
        var name = (state?.Name.Value ?? string.Empty).Trim().ToUpperInvariant();

        if (name == "CANCELADO" || name == "COMPLETADO")
            throw new Exception($"No se puede agregar un vuelo en estado '{state!.Name.Value}' a una reserva");
    }

    public async Task ValidateReservationAllowsChangesAsync(ReservationFlightReservationId reservationId)
    {
        var reservation = await _reservationRepository.GetByIdAsync(ReservationId.Create(reservationId.Value));
        if (reservation is null)
            throw new Exception("La reserva no existe");

        var status = await _reservationStatusRepository.GetByIdAsync(
            GestionAerolineas.src.Modules.ReservationStatuses.Domain.ValueObject.ReservationStatusId.Create(reservation.StatusId.Value));

        var name = (status?.Name.Value ?? string.Empty).Trim().ToUpperInvariant();
        if (name == "CANCELADA" || name == "VENCIDA")
            throw new Exception($"No se puede modificar vuelos en una reserva '{status!.Name.Value}'");
    }

    public async Task ValidateNoPassengersAsync(ReservationFlightId reservationFlightId)
    {
        var passengers = await _reservationPassengerRepository.GetByReservationFlightIdAsync(
            ReservationPassengerReservationFlightId.Create(reservationFlightId.Value));

        if (passengers.Any())
            throw new Exception("No se puede eliminar el vuelo de la reserva porque tiene pasajeros asociados (elimina reservationpassengers primero)");
    }
}
