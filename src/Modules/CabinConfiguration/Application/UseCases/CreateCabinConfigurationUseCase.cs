// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\CabinConfiguration\Application\UseCases\CreateCabinConfigurationUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.CabinConfiguration.Application.Interfaces;
using GestionAerolineas.src.Modules.CabinConfiguration.Domain.Aggregate;
using GestionAerolineas.src.Modules.CabinConfiguration.Domain.Repositories;
using GestionAerolineas.src.Modules.CabinConfiguration.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.CabinConfiguration.Application.UseCases;

public class CreateCabinConfigurationUseCase
{
    private readonly ICabinConfigurationRepository _repository;
    private readonly ICabinConfigurationValidator _validator;

    public CreateCabinConfigurationUseCase(ICabinConfigurationRepository repository, ICabinConfigurationValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(
        int aircraftId,
        int cabinTypeId,
        int startRow,
        int endRow,
        int seatsPerRow,
        string seatLetters)
    {
        var aircraftIdVO = CabinConfigurationAircraftId.Create(aircraftId);
        var cabinTypeIdVO = CabinConfigurationCabinTypeId.Create(cabinTypeId);
        var startRowVO = CabinConfigurationStartRow.Create(startRow);
        var endRowVO = CabinConfigurationEndRow.Create(endRow);
        var seatsPerRowVO = CabinConfigurationSeatsPerRow.Create(seatsPerRow);
        var seatLettersVO = CabinConfigurationSeatLetters.Create(seatLetters, seatsPerRowVO.Value);

        await _validator.ValidateAircraftExistsAsync(aircraftIdVO);
        await _validator.ValidateCabinTypeExistsAsync(cabinTypeIdVO);
        await _validator.ValidateUniqueCabinTypeInAircraftAsync(aircraftIdVO, cabinTypeIdVO);
        await _validator.ValidateRowRangeAsync(startRowVO, endRowVO);
        await _validator.ValidateSeatsAndLettersAsync(seatsPerRowVO, seatLettersVO);
        await _validator.ValidateNoRowOverlapAsync(aircraftIdVO, startRowVO, endRowVO);

        var entity = CabinConfigurationAggregate.CreateNew(
            aircraftIdVO,
            cabinTypeIdVO,
            startRowVO,
            endRowVO,
            seatsPerRowVO,
            seatLettersVO
        );

        await _repository.AddAsync(entity);
    }
}
