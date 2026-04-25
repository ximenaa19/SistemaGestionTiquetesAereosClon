// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\RouteStops\Application\UseCases\GetRouteStopByRouteAndOrderUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.RouteStops.Domain.Aggregate;
using GestionAerolineas.src.Modules.RouteStops.Domain.Repositories;
using GestionAerolineas.src.Modules.RouteStops.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.RouteStops.Application.UseCases;

public class GetRouteStopByRouteAndOrderUseCase
{
    private readonly IRouteStopRepository _repository;

    public GetRouteStopByRouteAndOrderUseCase(IRouteStopRepository repository)
    {
        _repository = repository;
    }

    public Task<RouteStop?> ExecuteAsync(int routeId, int order)
    {
        return _repository.GetByRouteAndOrderAsync(
            RouteStopRouteId.Create(routeId),
            RouteStopOrder.Create(order)
        );
    }
}

