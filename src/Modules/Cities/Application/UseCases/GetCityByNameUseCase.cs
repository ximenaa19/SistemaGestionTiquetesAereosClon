// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Cities\Application\UseCases\GetCityByNameUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Cities.Domain.Aggregate;
using GestionAerolineas.src.Modules.Cities.Domain.Repositories;
using GestionAerolineas.src.Modules.Cities.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Cities.Application.UseCases;

public class GetCityByNameUseCase
{
    private readonly ICityRepository _repository;

    public GetCityByNameUseCase(ICityRepository repository)
    {
        _repository = repository;
    }

    public Task<City?> ExecuteAsync(string name)
    {
        return _repository.GetByNameAsync(CityName.Create(name));
    }
}
