// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Countries\Application\UseCases\GetCountryByNameUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Countries.Domain.Aggregate;
using GestionAerolineas.src.Modules.Countries.Domain.Repositories;
using GestionAerolineas.src.Modules.Countries.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Countries.Application.UseCases;

public class GetCountryByNameUseCase
{
    private readonly ICountryRepository _repository;

    public GetCountryByNameUseCase(ICountryRepository repository)
    {
        _repository = repository;
    }

    public Task<Country?> ExecuteAsync(string name)
    {
        var nameVO = CountryName.Create(name);
        return _repository.GetByNameAsync(nameVO);
    }
}

