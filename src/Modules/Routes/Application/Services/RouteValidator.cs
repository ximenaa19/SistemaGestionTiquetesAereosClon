// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Routes\Application\Services\RouteValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Airports.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Routes.Application.Interfaces;
using GestionAerolineas.src.Modules.Routes.Domain.Repositories;
using GestionAerolineas.src.Modules.Routes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Routes.Application.Services;

public class RouteValidator : IRouteValidator
{
    private readonly IRouteRepository _repository;
    private readonly AirportRepository _airportRepository;

    public RouteValidator(IRouteRepository repository, AirportRepository airportRepository)
    {
        _repository = repository;
        _airportRepository = airportRepository;
    }

    public async Task ValidateAirportExistsAsync(RouteAirportId airportId)
    {
        var exists = await _airportRepository.ExistsAsync(
            GestionAerolineas.src.Modules.Airports.Domain.ValueObject.AirportId.Create(airportId.Value));

        if (!exists)
            throw new Exception("El aeropuerto no existe");
    }

    public Task ValidateDifferentAirportsAsync(RouteAirportId originAirportId, RouteAirportId destinationAirportId)
    {
        if (originAirportId.Value == destinationAirportId.Value)
            throw new Exception("El aeropuerto de origen y destino no pueden ser el mismo");

        return Task.CompletedTask;
    }

    public async Task ValidateUniquePairAsync(RouteAirportId originAirportId, RouteAirportId destinationAirportId, RouteId? currentId = null)
    {
        var exists = await _repository.ExistsByOriginAndDestinationAsync(
            originAirportId.Value,
            destinationAirportId.Value,
            currentId?.Value);

        if (exists)
            throw new Exception("Ya existe una ruta con ese origen y destino");
    }
}

