// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightSeats\Application\UseCases\UpdateFlightSeatUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.FlightSeats.Application.Interfaces;
using GestionAerolineas.src.Modules.FlightSeats.Domain.Aggregate;
using GestionAerolineas.src.Modules.FlightSeats.Domain.Repositories;
using GestionAerolineas.src.Modules.FlightSeats.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.FlightSeats.Application.UseCases;

public class UpdateFlightSeatUseCase
{
    private readonly IFlightSeatRepository _repository;
    private readonly IFlightSeatValidator _validator;

    public UpdateFlightSeatUseCase(IFlightSeatRepository repository, IFlightSeatValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(int id, int flightId, string seatCode, int cabinTypeId, int locationTypeId, bool isOccupied)
    {
        var idVO = FlightSeatId.Create(id);
        var flightIdVO = FlightSeatFlightId.Create(flightId);
        var codeVO = FlightSeatCode.Create(seatCode);
        var cabinTypeIdVO = FlightSeatCabinTypeId.Create(cabinTypeId);
        var locationTypeIdVO = FlightSeatLocationTypeId.Create(locationTypeId);
        var isOccupiedVO = FlightSeatIsOccupied.Create(isOccupied);

        await _validator.ValidateFlightExistsAsync(flightIdVO);
        await _validator.ValidateCabinTypeExistsAsync(cabinTypeIdVO);
        await _validator.ValidateLocationTypeExistsAsync(locationTypeIdVO);
        await _validator.ValidateUniqueSeatCodeInFlightAsync(flightIdVO, codeVO, idVO);

        var existing = await _repository.GetByIdAsync(idVO);
        if (existing is not null && existing.FlightId.Value != flightIdVO.Value)
            await _validator.ValidateSeatCountWithinFlightCapacityAsync(flightIdVO);

        var entity = FlightSeat.Create(idVO, flightIdVO, codeVO, cabinTypeIdVO, locationTypeIdVO, isOccupiedVO);
        await _repository.UpdateAsync(entity);
    }
}
