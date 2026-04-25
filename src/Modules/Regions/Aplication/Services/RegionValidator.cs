// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Regions\Aplication\Services\RegionValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Countries.Domain.ValueObject;
using GestionAerolineas.src.Modules.Countries.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Regions.Application.Interfaces;
using GestionAerolineas.src.Modules.Regions.Domain.Repositories;
using GestionAerolineas.src.Modules.Regions.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Regions.Application.Services;

public class RegionValidator : IRegionValidator
{
    private readonly IRegionRepository _repository;
    private readonly CountryRepository _countryRepository;

    public RegionValidator(IRegionRepository repository, CountryRepository countryRepository)
    {
        _repository = repository;
        _countryRepository = countryRepository;
    }

    public async Task ValidateCountryExistsAsync(RegionCountryId countryId)
    {
        var exists = await _countryRepository.ExistsAsync(CountryId.Create(countryId.Value));
        if (!exists)
            throw new Exception("El país no existe");
    }

    public async Task ValidateNameAsync(RegionName name, RegionCountryId countryId, RegionId? currentId = null)
    {
        var normalizedCandidate = RegionName.Normalize(name.Value);
        var exists = await _repository.ExistsByNormalizedNameInCountryAsync(
            normalizedCandidate,
            countryId.Value,
            currentId?.Value);

        if (exists)
            throw new Exception("Ya existe una región con ese nombre para este país");
    }
}
