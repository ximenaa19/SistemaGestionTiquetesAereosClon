// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Checkins\Application\UseCases\UpdateCheckinUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Checkins.Application.Interfaces;
using GestionAerolineas.src.Modules.Checkins.Domain.Aggregate;
using GestionAerolineas.src.Modules.Checkins.Domain.Repositories;
using GestionAerolineas.src.Modules.Checkins.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Checkins.Application.UseCases;

public class UpdateCheckinUseCase
{
    private readonly ICheckinRepository _repository;
    private readonly ICheckinValidator _validator;

    public UpdateCheckinUseCase(ICheckinRepository repository, ICheckinValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(
        int id,
        int ticketId,
        int staffId,
        int flightSeatId,
        DateTime checkedAt,
        int statusId,
        string? boardingPassNumber,
        bool hasHoldBaggage,
        decimal? baggageWeightKg)
    {
        var idVO = CheckinId.Create(id);
        var existing = await _repository.GetByIdAsync(idVO);
        if (existing is null)
            throw new Exception("El check-in no existe");

        var ticketIdVO = CheckinTicketId.Create(ticketId);
        var staffIdVO = CheckinStaffId.Create(staffId);
        var flightSeatIdVO = CheckinFlightSeatId.Create(flightSeatId);
        var checkedAtVO = CheckinCheckedAt.Create(checkedAt);
        var statusIdVO = CheckinStatusId.Create(statusId);
        var hasHoldBaggageVO = CheckinHasHoldBaggage.Create(hasHoldBaggage);
        var baggageWeightVO = CheckinBaggageWeightKg.Create(baggageWeightKg);

        var bp = string.IsNullOrWhiteSpace(boardingPassNumber)
            ? existing.BoardingPassNumber
            : CheckinBoardingPassNumber.Create(boardingPassNumber!);

        await _validator.ValidateTicketExistsAsync(ticketIdVO);
        await _validator.ValidateTicketUniqueAsync(ticketIdVO, idVO);
        await _validator.ValidateStaffExistsAsync(staffIdVO);
        await _validator.ValidateStaffIsActiveAirportStaffAsync(staffIdVO);
        await _validator.ValidateFlightSeatExistsAsync(flightSeatIdVO);
        await _validator.ValidateFlightSeatIsAvailableAsync(flightSeatIdVO, idVO);
        await _validator.ValidateStatusExistsAsync(statusIdVO);
        await _validator.ValidateBoardingPassUniqueAsync(bp, idVO);
        await _validator.ValidateSeatBelongsToTicketFlightAsync(ticketIdVO, flightSeatIdVO);
        await _validator.ValidateBaggageAsync(hasHoldBaggageVO, baggageWeightVO);

        var updated = Checkin.Create(
            idVO,
            ticketIdVO,
            staffIdVO,
            flightSeatIdVO,
            checkedAtVO,
            statusIdVO,
            bp,
            hasHoldBaggageVO,
            baggageWeightVO);

        await _repository.UpdateAsync(updated);
    }
}

