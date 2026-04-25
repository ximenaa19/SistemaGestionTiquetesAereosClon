// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Checkins\Application\Services\CheckinValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.CheckinStatuses.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Checkins.Application.Interfaces;
using GestionAerolineas.src.Modules.Checkins.Domain.Repositories;
using GestionAerolineas.src.Modules.Checkins.Domain.ValueObject;
using GestionAerolineas.src.Modules.FlightSeats.Domain.ValueObject;
using GestionAerolineas.src.Modules.FlightSeats.Infrastructure.Repository;
using GestionAerolineas.src.Modules.ReservationFlights.Domain.ValueObject;
using GestionAerolineas.src.Modules.ReservationFlights.Infrastructure.Repository;
using GestionAerolineas.src.Modules.ReservationPassengers.Domain.ValueObject;
using GestionAerolineas.src.Modules.ReservationPassengers.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Staff.Domain.ValueObject;
using GestionAerolineas.src.Modules.Staff.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Tickets.Domain.ValueObject;
using GestionAerolineas.src.Modules.Tickets.Infrastructure.Repository;

namespace GestionAerolineas.src.Modules.Checkins.Application.Services;

public class CheckinValidator : ICheckinValidator
{
    private readonly ICheckinRepository _checkinRepository;
    private readonly TicketRepository _ticketRepository;
    private readonly StaffRepository _staffRepository;
    private readonly FlightSeatRepository _flightSeatRepository;
    private readonly CheckinStatusRepository _checkinStatusRepository;
    private readonly ReservationPassengerRepository _reservationPassengerRepository;
    private readonly ReservationFlightRepository _reservationFlightRepository;

    public CheckinValidator(
        ICheckinRepository checkinRepository,
        TicketRepository ticketRepository,
        StaffRepository staffRepository,
        FlightSeatRepository flightSeatRepository,
        CheckinStatusRepository checkinStatusRepository,
        ReservationPassengerRepository reservationPassengerRepository,
        ReservationFlightRepository reservationFlightRepository)
    {
        _checkinRepository = checkinRepository;
        _ticketRepository = ticketRepository;
        _staffRepository = staffRepository;
        _flightSeatRepository = flightSeatRepository;
        _checkinStatusRepository = checkinStatusRepository;
        _reservationPassengerRepository = reservationPassengerRepository;
        _reservationFlightRepository = reservationFlightRepository;
    }

    public async Task ValidateTicketExistsAsync(CheckinTicketId ticketId)
    {
        var exists = await _ticketRepository.ExistsAsync(TicketId.Create(ticketId.Value));
        if (!exists)
            throw new Exception("El tiquete_id no existe");
    }

    public async Task ValidateTicketUniqueAsync(CheckinTicketId ticketId, CheckinId? excludingId = null)
    {
        var exists = await _checkinRepository.ExistsByTicketIdAsync(ticketId.Value, excludingId?.Value);
        if (exists)
            throw new Exception("Ya existe un check-in para este tiquete_id");
    }

    public async Task ValidateStaffExistsAsync(CheckinStaffId staffId)
    {
        var exists = await _staffRepository.ExistsAsync(StaffId.Create(staffId.Value));
        if (!exists)
            throw new Exception("El personal_id no existe");
    }

    public async Task ValidateStaffIsActiveAirportStaffAsync(CheckinStaffId staffId)
    {
        var staff = await _staffRepository.GetByIdAsync(StaffId.Create(staffId.Value));
        if (staff is null)
            throw new Exception("El personal_id no existe");
        if (!staff.IsActive.Value)
            throw new Exception("El personal esta inactivo");
        if (staff.AirportId.Value is null)
            throw new Exception("El personal no pertenece a un aeropuerto (aeropuerto_id es NULL)");
    }

    public async Task ValidateFlightSeatExistsAsync(CheckinFlightSeatId flightSeatId)
    {
        var exists = await _flightSeatRepository.ExistsAsync(FlightSeatId.Create(flightSeatId.Value));
        if (!exists)
            throw new Exception("El asiento_vuelo_id no existe");
    }

    public async Task ValidateFlightSeatIsAvailableAsync(CheckinFlightSeatId flightSeatId, CheckinId? excludingId = null)
    {
        var seat = await _flightSeatRepository.GetByIdAsync(FlightSeatId.Create(flightSeatId.Value));
        if (seat is null)
            throw new Exception("El asiento_vuelo_id no existe");

        if (excludingId is null && seat.IsOccupied.Value)
            throw new Exception("El asiento ya esta ocupado");

        var exists = await _checkinRepository.ExistsByFlightSeatIdAsync(flightSeatId.Value, excludingId?.Value);
        if (exists)
            throw new Exception("El asiento ya esta asignado a otro check-in");
    }

    public async Task ValidateStatusExistsAsync(CheckinStatusId statusId)
    {
        var exists = await _checkinStatusRepository.ExistsAsync(
            GestionAerolineas.src.Modules.CheckinStatuses.Domain.ValueObject.CheckinStatusId.Create(statusId.Value));
        if (!exists)
            throw new Exception("El estado_checkin_id no existe");
    }

    public async Task ValidateBoardingPassUniqueAsync(CheckinBoardingPassNumber boardingPass, CheckinId? excludingId = null)
    {
        var normalized = CheckinBoardingPassNumber.Normalize(boardingPass.Value);
        var exists = await _checkinRepository.ExistsByNormalizedBoardingPassAsync(normalized, excludingId?.Value);
        if (exists)
            throw new Exception("Ya existe un check-in con ese numero_tarjeta_embarque");
    }

    public async Task ValidateSeatBelongsToTicketFlightAsync(CheckinTicketId ticketId, CheckinFlightSeatId flightSeatId)
    {
        var ticket = await _ticketRepository.GetByIdAsync(TicketId.Create(ticketId.Value));
        if (ticket is null)
            throw new Exception("El tiquete_id no existe");

        var rp = await _reservationPassengerRepository.GetByIdAsync(ReservationPassengerId.Create(ticket.ReservationPassengerId.Value));
        if (rp is null)
            throw new Exception("El reserva_pasajero del ticket no existe");

        var rf = await _reservationFlightRepository.GetByIdAsync(ReservationFlightId.Create(rp.ReservationFlightId.Value));
        if (rf is null)
            throw new Exception("El reserva_vuelo del ticket no existe");

        var seat = await _flightSeatRepository.GetByIdAsync(FlightSeatId.Create(flightSeatId.Value));
        if (seat is null)
            throw new Exception("El asiento_vuelo_id no existe");

        if (seat.FlightId.Value != rf.FlightId.Value)
            throw new Exception("El asiento no pertenece al mismo vuelo del ticket");
    }

    public Task ValidateBaggageAsync(CheckinHasHoldBaggage hasHoldBaggage, CheckinBaggageWeightKg baggageWeightKg)
    {
        if (!hasHoldBaggage.Value && baggageWeightKg.Value != 0m)
            throw new Exception("Si equipaje_bodega=0, peso_equipaje_kg debe ser 0");
        if (hasHoldBaggage.Value && baggageWeightKg.Value <= 0m)
            throw new Exception("Si equipaje_bodega=1, peso_equipaje_kg debe ser > 0");
        return Task.CompletedTask;
    }
}
