// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Tickets\Application\Services\TicketValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.ReservationFlights.Domain.ValueObject;
using GestionAerolineas.src.Modules.ReservationFlights.Infrastructure.Repository;
using GestionAerolineas.src.Modules.ReservationPassengers.Domain.ValueObject;
using GestionAerolineas.src.Modules.ReservationPassengers.Infrastructure.Repository;
using GestionAerolineas.src.Modules.ReservationStatuses.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Reservations.Domain.ValueObject;
using GestionAerolineas.src.Modules.Reservations.Infrastructure.Repository;
using GestionAerolineas.src.Modules.TicketStatuses.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Tickets.Application.Interfaces;
using GestionAerolineas.src.Modules.Tickets.Domain.Repositories;
using GestionAerolineas.src.Modules.Tickets.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Tickets.Application.Services;

public class TicketValidator : ITicketValidator
{
    private readonly ITicketRepository _ticketRepository;
    private readonly ReservationPassengerRepository _reservationPassengerRepository;
    private readonly ReservationFlightRepository _reservationFlightRepository;
    private readonly ReservationRepository _reservationRepository;
    private readonly ReservationStatusRepository _reservationStatusRepository;
    private readonly TicketStatusRepository _ticketStatusRepository;

    public TicketValidator(
        ITicketRepository ticketRepository,
        ReservationPassengerRepository reservationPassengerRepository,
        ReservationFlightRepository reservationFlightRepository,
        ReservationRepository reservationRepository,
        ReservationStatusRepository reservationStatusRepository,
        TicketStatusRepository ticketStatusRepository)
    {
        _ticketRepository = ticketRepository;
        _reservationPassengerRepository = reservationPassengerRepository;
        _reservationFlightRepository = reservationFlightRepository;
        _reservationRepository = reservationRepository;
        _reservationStatusRepository = reservationStatusRepository;
        _ticketStatusRepository = ticketStatusRepository;
    }

    public async Task ValidateReservationPassengerExistsAsync(TicketReservationPassengerId reservationPassengerId)
    {
        var exists = await _reservationPassengerRepository.ExistsAsync(ReservationPassengerId.Create(reservationPassengerId.Value));
        if (!exists)
            throw new Exception("El reserva_pasajero_id no existe");
    }

    public async Task ValidateReservationPassengerIsUniqueAsync(TicketReservationPassengerId reservationPassengerId, TicketId? excludingId = null)
    {
        var exists = await _ticketRepository.ExistsByReservationPassengerIdAsync(reservationPassengerId.Value, excludingId?.Value);
        if (exists)
            throw new Exception("Ya existe un ticket para este reserva_pasajero_id");
    }

    public async Task ValidateTicketStatusExistsAsync(TicketStatusId statusId)
    {
        var exists = await _ticketStatusRepository.ExistsAsync(
            GestionAerolineas.src.Modules.TicketStatuses.Domain.ValueObject.TicketStatusId.Create(statusId.Value));
        if (!exists)
            throw new Exception("El estado_tiquete_id no existe");
    }

    public async Task ValidateReservationIsConfirmadaAsync(TicketReservationPassengerId reservationPassengerId)
    {
        var rp = await _reservationPassengerRepository.GetByIdAsync(ReservationPassengerId.Create(reservationPassengerId.Value));
        if (rp is null)
            throw new Exception("El reserva_pasajero_id no existe");

        var rf = await _reservationFlightRepository.GetByIdAsync(ReservationFlightId.Create(rp.ReservationFlightId.Value));
        if (rf is null)
            throw new Exception("El reserva_vuelo del pasajero no existe");

        var reservation = await _reservationRepository.GetByIdAsync(ReservationId.Create(rf.ReservationId.Value));
        if (reservation is null)
            throw new Exception("La reserva no existe");

        var status = await _reservationStatusRepository.GetByIdAsync(
            GestionAerolineas.src.Modules.ReservationStatuses.Domain.ValueObject.ReservationStatusId.Create(reservation.StatusId.Value));

        var name = (status?.Name.Value ?? string.Empty).Trim().ToUpperInvariant();
        if (name != "CONFIRMADA")
            throw new Exception("Solo se puede emitir ticket para reservas en estado 'Confirmada'");
    }

    public async Task ValidateTicketCodeUniqueAsync(TicketCode code, TicketId? excludingId = null)
    {
        var normalized = TicketCode.Normalize(code.Value);
        var exists = await _ticketRepository.ExistsByNormalizedCodeAsync(normalized, excludingId?.Value);
        if (exists)
            throw new Exception("Ya existe un ticket con ese codigo");
    }
}
