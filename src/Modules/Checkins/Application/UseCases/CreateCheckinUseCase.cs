// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Checkins\Application\UseCases\CreateCheckinUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System.Security.Cryptography;
using GestionAerolineas.src.Modules.Checkins.Application.Interfaces;
using GestionAerolineas.src.Modules.Checkins.Domain.Aggregate;
using GestionAerolineas.src.Modules.Checkins.Domain.Repositories;
using GestionAerolineas.src.Modules.Checkins.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Checkins.Application.UseCases;

public class CreateCheckinUseCase
{
    private readonly ICheckinRepository _repository;
    private readonly ICheckinValidator _validator;

    public CreateCheckinUseCase(ICheckinRepository repository, ICheckinValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<Checkin> ExecuteAsync(
        int ticketId,
        int staffId,
        int flightSeatId,
        DateTime? checkedAt,
        int statusId,
        bool hasHoldBaggage,
        decimal? baggageWeightKg)
    {
        var ticketIdVO = CheckinTicketId.Create(ticketId);
        var staffIdVO = CheckinStaffId.Create(staffId);
        var flightSeatIdVO = CheckinFlightSeatId.Create(flightSeatId);
        var checkedAtVO = CheckinCheckedAt.Create(checkedAt ?? DateTime.Now);
        var statusIdVO = CheckinStatusId.Create(statusId);
        var hasHoldBaggageVO = CheckinHasHoldBaggage.Create(hasHoldBaggage);
        var baggageWeightVO = CheckinBaggageWeightKg.Create(baggageWeightKg);

        await _validator.ValidateTicketExistsAsync(ticketIdVO);
        await _validator.ValidateTicketUniqueAsync(ticketIdVO);
        await _validator.ValidateStaffExistsAsync(staffIdVO);
        await _validator.ValidateStaffIsActiveAirportStaffAsync(staffIdVO);
        await _validator.ValidateFlightSeatExistsAsync(flightSeatIdVO);
        await _validator.ValidateFlightSeatIsAvailableAsync(flightSeatIdVO);
        await _validator.ValidateStatusExistsAsync(statusIdVO);
        await _validator.ValidateSeatBelongsToTicketFlightAsync(ticketIdVO, flightSeatIdVO);
        await _validator.ValidateBaggageAsync(hasHoldBaggageVO, baggageWeightVO);

        var boardingPass = await GenerateUniqueBoardingPassAsync();
        var entity = Checkin.CreateNew(
            ticketIdVO,
            staffIdVO,
            flightSeatIdVO,
            checkedAtVO,
            statusIdVO,
            boardingPass,
            hasHoldBaggageVO,
            baggageWeightVO);

        await _repository.AddAsync(entity);

        var created = await _repository.GetByTicketIdAsync(ticketIdVO);
        return created ?? entity;
    }

    private async Task<CheckinBoardingPassNumber> GenerateUniqueBoardingPassAsync()
    {
        for (int attempt = 0; attempt < 10; attempt++)
        {
            var suffix = RandomNumberGenerator.GetInt32(0, 1000);
            var candidate = $"BP{DateTime.Now:yyMMddHHmmss}{suffix:000}";
            var vo = CheckinBoardingPassNumber.Create(candidate);
            var exists = await _repository.ExistsByNormalizedBoardingPassAsync(CheckinBoardingPassNumber.Normalize(vo.Value));
            if (!exists)
                return vo;
        }

        throw new Exception("No se pudo generar un numero_tarjeta_embarque unico");
    }
}

