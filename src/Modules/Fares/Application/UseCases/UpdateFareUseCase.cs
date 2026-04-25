// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Fares\Application\UseCases\UpdateFareUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Fares.Application.Interfaces;
using GestionAerolineas.src.Modules.Fares.Domain.Aggregate;
using GestionAerolineas.src.Modules.Fares.Domain.Repositories;
using GestionAerolineas.src.Modules.Fares.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Fares.Application.UseCases;

public class UpdateFareUseCase
{
    private readonly IFareRepository _repository;
    private readonly IFareValidator _validator;

    public UpdateFareUseCase(IFareRepository repository, IFareValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(
        int id,
        int routeId,
        int cabinTypeId,
        int passengerTypeId,
        int seasonId,
        decimal basePrice,
        DateTime? validFrom,
        DateTime? validUntil)
    {
        var idVO = FareId.Create(id);
        var routeIdVO = FareRouteId.Create(routeId);
        var cabinTypeIdVO = FareCabinTypeId.Create(cabinTypeId);
        var passengerTypeIdVO = FarePassengerTypeId.Create(passengerTypeId);
        var seasonIdVO = FareSeasonId.Create(seasonId);
        var basePriceVO = FareBasePrice.Create(basePrice);
        var validFromVO = FareValidFromDate.Create(validFrom);
        var validUntilVO = FareValidUntilDate.Create(validUntil);

        await _validator.ValidateRouteExistsAsync(routeIdVO);
        await _validator.ValidateCabinTypeExistsAsync(cabinTypeIdVO);
        await _validator.ValidatePassengerTypeExistsAsync(passengerTypeIdVO);
        await _validator.ValidateSeasonExistsAsync(seasonIdVO);
        await _validator.ValidateUniqueKeysAsync(routeIdVO, cabinTypeIdVO, passengerTypeIdVO, seasonIdVO, idVO);
        _validator.ValidateValidFromBeforeValidUntil(validFromVO, validUntilVO);

        var entity = Fare.Create(idVO, routeIdVO, cabinTypeIdVO, passengerTypeIdVO, seasonIdVO, basePriceVO, validFromVO, validUntilVO);
        await _repository.UpdateAsync(entity);
    }
}

