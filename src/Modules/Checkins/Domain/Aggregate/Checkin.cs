// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Checkins\Domain\Aggregate\Checkin.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Checkins.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Checkins.Domain.Aggregate;

public class Checkin
{
    public CheckinId Id { get; private set; }
    public CheckinTicketId TicketId { get; private set; }
    public CheckinStaffId StaffId { get; private set; }
    public CheckinFlightSeatId FlightSeatId { get; private set; }
    public CheckinCheckedAt CheckedAt { get; private set; }
    public CheckinStatusId StatusId { get; private set; }
    public CheckinBoardingPassNumber BoardingPassNumber { get; private set; }
    public CheckinHasHoldBaggage HasHoldBaggage { get; private set; }
    public CheckinBaggageWeightKg BaggageWeightKg { get; private set; }

    private Checkin(
        CheckinId id,
        CheckinTicketId ticketId,
        CheckinStaffId staffId,
        CheckinFlightSeatId flightSeatId,
        CheckinCheckedAt checkedAt,
        CheckinStatusId statusId,
        CheckinBoardingPassNumber boardingPassNumber,
        CheckinHasHoldBaggage hasHoldBaggage,
        CheckinBaggageWeightKg baggageWeightKg)
    {
        Id = id;
        TicketId = ticketId;
        StaffId = staffId;
        FlightSeatId = flightSeatId;
        CheckedAt = checkedAt;
        StatusId = statusId;
        BoardingPassNumber = boardingPassNumber;
        HasHoldBaggage = hasHoldBaggage;
        BaggageWeightKg = baggageWeightKg;
    }

    public static Checkin Create(
        CheckinId id,
        CheckinTicketId ticketId,
        CheckinStaffId staffId,
        CheckinFlightSeatId flightSeatId,
        CheckinCheckedAt checkedAt,
        CheckinStatusId statusId,
        CheckinBoardingPassNumber boardingPassNumber,
        CheckinHasHoldBaggage hasHoldBaggage,
        CheckinBaggageWeightKg baggageWeightKg)
    {
        return new Checkin(
            id,
            ticketId,
            staffId,
            flightSeatId,
            checkedAt,
            statusId,
            boardingPassNumber,
            hasHoldBaggage,
            baggageWeightKg);
    }

    public static Checkin CreateNew(
        CheckinTicketId ticketId,
        CheckinStaffId staffId,
        CheckinFlightSeatId flightSeatId,
        CheckinCheckedAt checkedAt,
        CheckinStatusId statusId,
        CheckinBoardingPassNumber boardingPassNumber,
        CheckinHasHoldBaggage hasHoldBaggage,
        CheckinBaggageWeightKg baggageWeightKg)
    {
        return new Checkin(
            CheckinId.CreateEmpty(),
            ticketId,
            staffId,
            flightSeatId,
            checkedAt,
            statusId,
            boardingPassNumber,
            hasHoldBaggage,
            baggageWeightKg);
    }
}

