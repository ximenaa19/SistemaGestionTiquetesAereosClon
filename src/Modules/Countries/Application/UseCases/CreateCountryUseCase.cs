// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Countries\Application\UseCases\CreateCountryUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Countries.Application.Interfaces;
using GestionAerolineas.src.Modules.Countries.Domain.Aggregate;
using GestionAerolineas.src.Modules.Countries.Domain.Repositories;
using GestionAerolineas.src.Modules.Countries.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Countries.Application.UseCases;

public class CreateCountryUseCase
{
    private readonly ICountryRepository _repository;
    private readonly ICountryValidator _validator;

    public CreateCountryUseCase(ICountryRepository repository, ICountryValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(string name, string isoCode, int continentId)
    {
        var nameVO = CountryName.Create(name);
        var isoVO = CountryCodigoIso.Create(isoCode);
        var continentVO = CountryContinentId.Create(continentId);

        await _validator.ValidateContinentExistsAsync(continentVO);
        await _validator.ValidateIsoCodeAsync(isoVO);

        var entity = Country.CreateNew(nameVO, isoVO, continentVO);

        await _repository.AddAsync(entity);
    }
}

