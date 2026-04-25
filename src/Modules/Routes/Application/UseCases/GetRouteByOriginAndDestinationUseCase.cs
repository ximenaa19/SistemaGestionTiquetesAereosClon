// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Routes\Application\UseCases\GetRouteByOriginAndDestinationUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Routes.Domain.Aggregate;
using GestionAerolineas.src.Modules.Routes.Domain.Repositories;
using GestionAerolineas.src.Modules.Routes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Routes.Application.UseCases;

public class GetRouteByOriginAndDestinationUseCase
{
    private readonly IRouteRepository _repository;

    public GetRouteByOriginAndDestinationUseCase(IRouteRepository repository)
    {
        _repository = repository;
    }

    public Task<Route?> ExecuteAsync(int originAirportId, int destinationAirportId)
    {
        return _repository.GetByOriginAndDestinationAsync(
            RouteAirportId.Create(originAirportId),
            RouteAirportId.Create(destinationAirportId)
        );
    }
}

