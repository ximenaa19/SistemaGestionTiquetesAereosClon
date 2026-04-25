// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Countries\Application\Services\CountryValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Continents.Domain.ValueObject;
using GestionAerolineas.src.Modules.Continents.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Countries.Application.Interfaces;
using GestionAerolineas.src.Modules.Countries.Domain.Repositories;
using GestionAerolineas.src.Modules.Countries.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Countries.Application.Services;

public class CountryValidator : ICountryValidator
{
    private readonly ICountryRepository _repository;
    private readonly ContinentRepository _continentRepository;

    public CountryValidator(ICountryRepository repository, ContinentRepository continentRepository)
    {
        _repository = repository;
        _continentRepository = continentRepository;
    }

    public async Task ValidateIsoCodeAsync(CountryCodigoIso isoCode, CountryId? currentId = null)
    {
        var all = await _repository.GetAllAsync();
        var candidate = CountryCodigoIso.Normalize(isoCode.Value);

        foreach (var item in all)
        {
            if (currentId != null && item.Id.Value == currentId.Value)
                continue;

            if (CountryCodigoIso.Normalize(item.IsoCode.Value) == candidate)
                throw new Exception("Ya existe un país con ese código ISO");
        }
    }

    public async Task ValidateContinentExistsAsync(CountryContinentId continentId)
    {
        var exists = await _continentRepository.ExistsAsync(ContinentId.Create(continentId.Value));
        if (!exists)
            throw new Exception("El continente no existe");
    }
}

