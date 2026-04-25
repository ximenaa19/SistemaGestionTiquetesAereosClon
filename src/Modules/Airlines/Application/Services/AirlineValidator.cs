// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Airlines\Application\Services\AirlineValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Airlines.Application.Interfaces;
using GestionAerolineas.src.Modules.Airlines.Domain.Repositories;
using GestionAerolineas.src.Modules.Airlines.Domain.ValueObject;
using GestionAerolineas.src.Modules.Countries.Domain.ValueObject;
using GestionAerolineas.src.Modules.Countries.Infrastructure.Repository;

namespace GestionAerolineas.src.Modules.Airlines.Application.Services;

public class AirlineValidator : IAirlineValidator
{
    private readonly IAirlineRepository _repository;
    private readonly CountryRepository _countryRepository;

    public AirlineValidator(IAirlineRepository repository, CountryRepository countryRepository)
    {
        _repository = repository;
        _countryRepository = countryRepository;
    }

    public async Task ValidateOriginCountryExistsAsync(AirlineOriginCountryId originCountryId)
    {
        var exists = await _countryRepository.ExistsAsync(CountryId.Create(originCountryId.Value));
        if (!exists)
            throw new Exception("El pais de origen no existe");
    }

    public async Task ValidateNameAsync(AirlineName name, AirlineOriginCountryId originCountryId, AirlineId? currentId = null)
    {
        var normalizedCandidate = AirlineName.Normalize(name.Value);
        var exists = await _repository.ExistsByNormalizedNameInOriginCountryAsync(
            normalizedCandidate,
            originCountryId.Value,
            currentId?.Value);

        if (exists)
            throw new Exception("Ya existe una aerolinea con ese nombre para este pais");
    }

    public async Task ValidateIataCodeAsync(AirlineIataCode iataCode, AirlineId? currentId = null)
    {
        var normalizedCandidate = AirlineIataCode.Normalize(iataCode.Value);
        var exists = await _repository.ExistsByNormalizedIataCodeAsync(normalizedCandidate, currentId?.Value);

        if (exists)
            throw new Exception("Ya existe una aerolinea con ese codigo IATA");
    }
}

