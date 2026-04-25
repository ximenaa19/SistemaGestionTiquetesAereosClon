// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Airports\Application\Services\AirportValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Airports.Application.Interfaces;
using GestionAerolineas.src.Modules.Airports.Domain.Repositories;
using GestionAerolineas.src.Modules.Airports.Domain.ValueObject;
using GestionAerolineas.src.Modules.Cities.Domain.ValueObject;
using GestionAerolineas.src.Modules.Cities.Infrastructure.Repository;

namespace GestionAerolineas.src.Modules.Airports.Application.Services;

public class AirportValidator : IAirportValidator
{
    private readonly IAirportRepository _repository;
    private readonly CityRepository _cityRepository;

    public AirportValidator(IAirportRepository repository, CityRepository cityRepository)
    {
        _repository = repository;
        _cityRepository = cityRepository;
    }

    public async Task ValidateCityExistsAsync(AirportCityId cityId)
    {
        var exists = await _cityRepository.ExistsAsync(CityId.Create(cityId.Value));
        if (!exists)
            throw new Exception("La ciudad no existe");
    }

    public async Task ValidateNameAsync(AirportName name, AirportCityId cityId, AirportId? currentId = null)
    {
        var normalizedCandidate = AirportName.Normalize(name.Value);
        var exists = await _repository.ExistsByNormalizedNameInCityAsync(
            normalizedCandidate,
            cityId.Value,
            currentId?.Value);

        if (exists)
            throw new Exception("Ya existe un aeropuerto con ese nombre para esta ciudad");
    }

    public async Task ValidateIataCodeAsync(AirportIataCode iataCode, AirportId? currentId = null)
    {
        var exists = await _repository.ExistsByNormalizedIataCodeAsync(
            AirportIataCode.Normalize(iataCode.Value),
            currentId?.Value);

        if (exists)
            throw new Exception("Ya existe un aeropuerto con ese codigo IATA");
    }

    public async Task ValidateIcaoCodeAsync(AirportIcaoCode? icaoCode, AirportId? currentId = null)
    {
        if (icaoCode is null)
            return;

        var exists = await _repository.ExistsByNormalizedIcaoCodeAsync(
            AirportIcaoCode.Normalize(icaoCode.Value),
            currentId?.Value);

        if (exists)
            throw new Exception("Ya existe un aeropuerto con ese codigo ICAO");
    }
}
