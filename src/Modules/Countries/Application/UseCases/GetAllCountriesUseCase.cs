// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Countries\Application\UseCases\GetAllCountriesUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Countries.Domain.Aggregate;
using GestionAerolineas.src.Modules.Countries.Domain.Repositories;

namespace GestionAerolineas.src.Modules.Countries.Application.UseCases;

public class GetAllCountriesUseCase
{
    private readonly ICountryRepository _repository;

    public GetAllCountriesUseCase(ICountryRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Country>> ExecuteAsync()
    {
        return _repository.GetAllAsync();
    }
}

