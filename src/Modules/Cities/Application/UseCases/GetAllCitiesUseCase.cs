// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Cities\Application\UseCases\GetAllCitiesUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Cities.Domain.Aggregate;
using GestionAerolineas.src.Modules.Cities.Domain.Repositories;

namespace GestionAerolineas.src.Modules.Cities.Application.UseCases;

public class GetAllCitiesUseCase
{
    private readonly ICityRepository _repository;

    public GetAllCitiesUseCase(ICityRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<City>> ExecuteAsync()
    {
        return _repository.GetAllAsync();
    }
}
