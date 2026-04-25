// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Cities\Application\Services\CityValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Cities.Application.Interfaces;
using GestionAerolineas.src.Modules.Cities.Domain.Repositories;
using GestionAerolineas.src.Modules.Cities.Domain.ValueObject;
using GestionAerolineas.src.Modules.Regions.Domain.ValueObject;
using GestionAerolineas.src.Modules.Regions.Infrastructure.Repository;

namespace GestionAerolineas.src.Modules.Cities.Application.Services;

public class CityValidator : ICityValidator
{
    private readonly ICityRepository _repository;
    private readonly RegionRepository _regionRepository;

    public CityValidator(ICityRepository repository, RegionRepository regionRepository)
    {
        _repository = repository;
        _regionRepository = regionRepository;
    }

    public async Task ValidateRegionExistsAsync(CityRegionId regionId)
    {
        var exists = await _regionRepository.ExistsAsync(RegionId.Create(regionId.Value));
        if (!exists)
            throw new Exception("La region no existe");
    }

    public async Task ValidateNameAsync(CityName name, CityRegionId regionId, CityId? currentId = null)
    {
        var normalizedCandidate = CityName.Normalize(name.Value);
        var exists = await _repository.ExistsByNormalizedNameInRegionAsync(
            normalizedCandidate,
            regionId.Value,
            currentId?.Value);

        if (exists)
            throw new Exception("Ya existe una ciudad con ese nombre para esta region");
    }
}
