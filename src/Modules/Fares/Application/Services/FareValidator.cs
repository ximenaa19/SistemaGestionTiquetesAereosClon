// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Fares\Application\Services\FareValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.CabinTypes.Domain.ValueObject;
using GestionAerolineas.src.Modules.CabinTypes.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Fares.Application.Interfaces;
using GestionAerolineas.src.Modules.Fares.Domain.Repositories;
using GestionAerolineas.src.Modules.Fares.Domain.ValueObject;
using GestionAerolineas.src.Modules.PassengerTypes.Domain.ValueObject;
using GestionAerolineas.src.Modules.PassengerTypes.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Routes.Domain.ValueObject;
using GestionAerolineas.src.Modules.Routes.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Seasons.Domain.ValueObject;
using GestionAerolineas.src.Modules.Seasons.Infrastructure.Repository;

namespace GestionAerolineas.src.Modules.Fares.Application.Services;

public class FareValidator : IFareValidator
{
    private readonly IFareRepository _repository;
    private readonly RouteRepository _routeRepository;
    private readonly CabinTypeRepository _cabinTypeRepository;
    private readonly PassengerTypeRepository _passengerTypeRepository;
    private readonly SeasonRepository _seasonRepository;

    public FareValidator(
        IFareRepository repository,
        RouteRepository routeRepository,
        CabinTypeRepository cabinTypeRepository,
        PassengerTypeRepository passengerTypeRepository,
        SeasonRepository seasonRepository)
    {
        _repository = repository;
        _routeRepository = routeRepository;
        _cabinTypeRepository = cabinTypeRepository;
        _passengerTypeRepository = passengerTypeRepository;
        _seasonRepository = seasonRepository;
    }

    public async Task ValidateRouteExistsAsync(FareRouteId routeId)
    {
        var exists = await _routeRepository.ExistsAsync(RouteId.Create(routeId.Value));
        if (!exists)
            throw new Exception("La ruta no existe");
    }

    public async Task ValidateCabinTypeExistsAsync(FareCabinTypeId cabinTypeId)
    {
        var exists = await _cabinTypeRepository.ExistsAsync(CabinTypesId.Create(cabinTypeId.Value));
        if (!exists)
            throw new Exception("El tipo de cabina no existe");
    }

    public async Task ValidatePassengerTypeExistsAsync(FarePassengerTypeId passengerTypeId)
    {
        var exists = await _passengerTypeRepository.ExistsAsync(PassengerTypeId.Create(passengerTypeId.Value));
        if (!exists)
            throw new Exception("El tipo de pasajero no existe");
    }

    public async Task ValidateSeasonExistsAsync(FareSeasonId seasonId)
    {
        var exists = await _seasonRepository.ExistsAsync(SeasonId.Create(seasonId.Value));
        if (!exists)
            throw new Exception("La temporada no existe");
    }

    public async Task ValidateUniqueKeysAsync(
        FareRouteId routeId,
        FareCabinTypeId cabinTypeId,
        FarePassengerTypeId passengerTypeId,
        FareSeasonId seasonId,
        FareId? currentId = null)
    {
        var exists = await _repository.ExistsByKeysAsync(
            routeId.Value,
            cabinTypeId.Value,
            passengerTypeId.Value,
            seasonId.Value,
            currentId?.Value);

        if (exists)
            throw new Exception("Ya existe una tarifa con esa combinacion (ruta + tipo_cabina + tipo_pasajero + temporada)");
    }

    public void ValidateValidFromBeforeValidUntil(FareValidFromDate validFrom, FareValidUntilDate validUntil)
    {
        if (validFrom.Value.HasValue && validUntil.Value.HasValue && validUntil.Value.Value < validFrom.Value.Value)
            throw new Exception("La vigencia_hasta no puede ser menor que la vigencia_desde");
    }
}

