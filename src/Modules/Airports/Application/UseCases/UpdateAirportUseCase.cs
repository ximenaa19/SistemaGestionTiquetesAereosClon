// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Airports\Application\UseCases\UpdateAirportUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Airports.Application.Interfaces;
using GestionAerolineas.src.Modules.Airports.Domain.Aggregate;
using GestionAerolineas.src.Modules.Airports.Domain.Repositories;
using GestionAerolineas.src.Modules.Airports.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Airports.Application.UseCases;

public class UpdateAirportUseCase
{
    private readonly IAirportRepository _repository;
    private readonly IAirportValidator _validator;

    public UpdateAirportUseCase(IAirportRepository repository, IAirportValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(int id, string name, string iataCode, string? icaoCode, int cityId)
    {
        var idVO = AirportId.Create(id);
        var nameVO = AirportName.Create(name);
        var iataVO = AirportIataCode.Create(iataCode);
        var icaoVO = AirportIcaoCode.CreateOptional(icaoCode);
        var cityVO = AirportCityId.Create(cityId);

        await _validator.ValidateCityExistsAsync(cityVO);
        await _validator.ValidateNameAsync(nameVO, cityVO, idVO);
        await _validator.ValidateIataCodeAsync(iataVO, idVO);
        await _validator.ValidateIcaoCodeAsync(icaoVO, idVO);

        var entity = Airport.Create(idVO, nameVO, iataVO, icaoVO, cityVO);
        await _repository.UpdateAsync(entity);
    }
}
