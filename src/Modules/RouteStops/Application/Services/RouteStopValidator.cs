// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\RouteStops\Application\Services\RouteStopValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Airports.Domain.ValueObject;
using GestionAerolineas.src.Modules.Airports.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Routes.Domain.ValueObject;
using GestionAerolineas.src.Modules.Routes.Infrastructure.Repository;
using GestionAerolineas.src.Modules.RouteStops.Application.Interfaces;
using GestionAerolineas.src.Modules.RouteStops.Domain.Repositories;
using GestionAerolineas.src.Modules.RouteStops.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.RouteStops.Application.Services;

public class RouteStopValidator : IRouteStopValidator
{
    private readonly IRouteStopRepository _repository;
    private readonly RouteRepository _routeRepository;
    private readonly AirportRepository _airportRepository;

    public RouteStopValidator(
        IRouteStopRepository repository,
        RouteRepository routeRepository,
        AirportRepository airportRepository)
    {
        _repository = repository;
        _routeRepository = routeRepository;
        _airportRepository = airportRepository;
    }

    public async Task ValidateRouteExistsAsync(RouteStopRouteId routeId)
    {
        var exists = await _routeRepository.ExistsAsync(RouteId.Create(routeId.Value));
        if (!exists)
            throw new Exception("La ruta no existe");
    }

    public async Task ValidateStopAirportExistsAsync(RouteStopStopAirportId stopAirportId)
    {
        var exists = await _airportRepository.ExistsAsync(AirportId.Create(stopAirportId.Value));
        if (!exists)
            throw new Exception("El aeropuerto de escala no existe");
    }

    public async Task ValidateUniqueOrderInRouteAsync(RouteStopRouteId routeId, RouteStopOrder order, RouteStopId? currentId = null)
    {
        var exists = await _repository.ExistsByRouteAndOrderAsync(routeId.Value, order.Value, currentId?.Value);
        if (exists)
            throw new Exception("Ya existe una escala con ese orden para esta ruta");
    }

    public async Task ValidateNoDuplicateStopAirportInRouteAsync(RouteStopRouteId routeId, RouteStopStopAirportId stopAirportId, RouteStopId? currentId = null)
    {
        var exists = await _repository.ExistsByRouteAndStopAirportAsync(routeId.Value, stopAirportId.Value, currentId?.Value);
        if (exists)
            throw new Exception("Ese aeropuerto de escala ya existe para esta ruta");
    }

    public async Task ValidateStopAirportNotOriginOrDestinationAsync(RouteStopRouteId routeId, RouteStopStopAirportId stopAirportId)
    {
        var route = await _routeRepository.GetByIdAsync(RouteId.Create(routeId.Value));
        if (route is null)
            throw new Exception("La ruta no existe");

        if (route.OriginAirportId.Value == stopAirportId.Value)
            throw new Exception("El aeropuerto de escala no puede ser igual al aeropuerto de origen");

        if (route.DestinationAirportId.Value == stopAirportId.Value)
            throw new Exception("El aeropuerto de escala no puede ser igual al aeropuerto de destino");
    }
}

