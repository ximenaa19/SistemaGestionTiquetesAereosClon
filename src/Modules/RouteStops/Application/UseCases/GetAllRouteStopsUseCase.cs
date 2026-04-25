// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\RouteStops\Application\UseCases\GetAllRouteStopsUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.RouteStops.Domain.Aggregate;
using GestionAerolineas.src.Modules.RouteStops.Domain.Repositories;

namespace GestionAerolineas.src.Modules.RouteStops.Application.UseCases;

public class GetAllRouteStopsUseCase
{
    private readonly IRouteStopRepository _repository;

    public GetAllRouteStopsUseCase(IRouteStopRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<RouteStop>> ExecuteAsync()
    {
        return _repository.GetAllAsync();
    }
}

