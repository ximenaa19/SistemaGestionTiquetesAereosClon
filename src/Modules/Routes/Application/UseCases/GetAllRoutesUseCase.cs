// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Routes\Application\UseCases\GetAllRoutesUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Routes.Domain.Aggregate;
using GestionAerolineas.src.Modules.Routes.Domain.Repositories;

namespace GestionAerolineas.src.Modules.Routes.Application.UseCases;

public class GetAllRoutesUseCase
{
    private readonly IRouteRepository _repository;

    public GetAllRoutesUseCase(IRouteRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Route>> ExecuteAsync()
    {
        return _repository.GetAllAsync();
    }
}

