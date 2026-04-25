// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\AircraftManufacturers\Application\Services\AircraftManufacturerValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.AircraftManufacturers.Application.Interfaces;
using GestionAerolineas.src.Modules.AircraftManufacturers.Domain.Repositories;
using GestionAerolineas.src.Modules.AircraftManufacturers.Domain.ValueObject;
using GestionAerolineas.src.Modules.Countries.Domain.ValueObject;
using GestionAerolineas.src.Modules.Countries.Infrastructure.Repository;

namespace GestionAerolineas.src.Modules.AircraftManufacturers.Application.Services;

public class AircraftManufacturerValidator : IAircraftManufacturerValidator
{
    private readonly IAircraftManufacturerRepository _repository;
    private readonly CountryRepository _countryRepository;

    public AircraftManufacturerValidator(IAircraftManufacturerRepository repository, CountryRepository countryRepository)
    {
        _repository = repository;
        _countryRepository = countryRepository;
    }

    public async Task ValidateNameAsync(AircraftManufacturerName name, AircraftManufacturerId? currentId = null)
    {
        var normalizedCandidate = AircraftManufacturerName.Normalize(name.Value);
        var all = await _repository.GetAllAsync();

        foreach (var item in all)
        {
            if (currentId != null && item.Id.Value == currentId.Value)
                continue;

            if (AircraftManufacturerName.Normalize(item.Name.Value) == normalizedCandidate)
                throw new Exception("Ya existe un fabricante con ese nombre");
        }
    }

    public async Task ValidateCountryExistsAsync(AircraftManufacturerCountryId countryId)
    {
        var exists = await _countryRepository.ExistsAsync(CountryId.Create(countryId.Value));
        if (!exists)
            throw new Exception("El país no existe");
    }
}

