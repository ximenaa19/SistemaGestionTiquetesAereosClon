// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\AirportAirline\Application\UseCases\UpdateAirportAirlineUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.AirportAirline.Application.Interfaces;
using GestionAerolineas.src.Modules.AirportAirline.Domain.Aggregate;
using GestionAerolineas.src.Modules.AirportAirline.Domain.Repositories;
using GestionAerolineas.src.Modules.AirportAirline.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.AirportAirline.Application.UseCases;

public class UpdateAirportAirlineUseCase
{
    private readonly IAirportAirlineRepository _repository;
    private readonly IAirportAirlineValidator _validator;

    public UpdateAirportAirlineUseCase(IAirportAirlineRepository repository, IAirportAirlineValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(int id, int airportId, int airlineId, string? terminal, DateTime startDate, DateTime? endDate, bool isActive)
    {
        var idVO = AirportAirlineId.Create(id);
        var airportIdVO = AirportAirlineAirportId.Create(airportId);
        var airlineIdVO = AirportAirlineAirlineId.Create(airlineId);
        var terminalVO = AirportAirlineTerminal.Create(terminal);
        var startVO = AirportAirlineStartDate.Create(startDate);
        var endVO = AirportAirlineEndDate.Create(endDate);
        var isActiveVO = AirportAirlineIsActive.Create(isActive);

        await _validator.ValidateAirportExistsAsync(airportIdVO);
        await _validator.ValidateAirlineExistsAsync(airlineIdVO);
        await _validator.ValidateUniquePairAsync(airportIdVO, airlineIdVO, idVO);
        await _validator.ValidateDatesAsync(startVO, endVO);

        var entity = AirportAirlineRelation.Create(idVO, airportIdVO, airlineIdVO, terminalVO, startVO, endVO, isActiveVO);
        await _repository.UpdateAsync(entity);
    }
}

