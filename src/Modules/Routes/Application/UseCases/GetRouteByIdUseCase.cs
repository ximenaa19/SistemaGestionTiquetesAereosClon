// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Routes\Application\UseCases\GetRouteByIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Routes.Domain.Aggregate;
using GestionAerolineas.src.Modules.Routes.Domain.Repositories;
using GestionAerolineas.src.Modules.Routes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Routes.Application.UseCases;

public class GetRouteByIdUseCase
{
    private readonly IRouteRepository _repository;

    public GetRouteByIdUseCase(IRouteRepository repository)
    {
        _repository = repository;
    }

    public Task<Route?> ExecuteAsync(int id)
    {
        return _repository.GetByIdAsync(RouteId.Create(id));
    }
}

