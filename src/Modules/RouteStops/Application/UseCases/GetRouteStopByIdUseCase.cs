// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\RouteStops\Application\UseCases\GetRouteStopByIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.RouteStops.Domain.Aggregate;
using GestionAerolineas.src.Modules.RouteStops.Domain.Repositories;
using GestionAerolineas.src.Modules.RouteStops.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.RouteStops.Application.UseCases;

public class GetRouteStopByIdUseCase
{
    private readonly IRouteStopRepository _repository;

    public GetRouteStopByIdUseCase(IRouteStopRepository repository)
    {
        _repository = repository;
    }

    public Task<RouteStop?> ExecuteAsync(int id)
    {
        return _repository.GetByIdAsync(RouteStopId.Create(id));
    }
}

