// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Aircraft\Application\Services\AircraftValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Aircraft.Application.Interfaces;
using GestionAerolineas.src.Modules.Aircraft.Domain.Repositories;
using GestionAerolineas.src.Modules.Aircraft.Domain.ValueObject;
using GestionAerolineas.src.Modules.AircraftModels.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Airlines.Infrastructure.Repository;

namespace GestionAerolineas.src.Modules.Aircraft.Application.Services;

public class AircraftValidator : IAircraftValidator
{
    private readonly IAircraftRepository _repository;
    private readonly AircraftModelRepository _aircraftModelRepository;
    private readonly AirlineRepository _airlineRepository;

    public AircraftValidator(
        IAircraftRepository repository,
        AircraftModelRepository aircraftModelRepository,
        AirlineRepository airlineRepository)
    {
        _repository = repository;
        _aircraftModelRepository = aircraftModelRepository;
        _airlineRepository = airlineRepository;
    }

    public async Task ValidateModelExistsAsync(AircraftModelId modelId)
    {
        var exists = await _aircraftModelRepository.ExistsAsync(
            GestionAerolineas.src.Modules.AircraftModels.Domain.ValueObject.AircraftModelId.Create(modelId.Value));

        if (!exists)
            throw new Exception("El modelo de aeronave no existe");
    }

    public async Task ValidateAirlineExistsAsync(AircraftAirlineId airlineId)
    {
        var exists = await _airlineRepository.ExistsAsync(
            GestionAerolineas.src.Modules.Airlines.Domain.ValueObject.AirlineId.Create(airlineId.Value));

        if (!exists)
            throw new Exception("La aerolinea no existe");
    }

    public async Task ValidateRegistrationAsync(AircraftRegistration registration, AircraftId? currentId = null)
    {
        var normalizedCandidate = AircraftRegistration.Normalize(registration.Value);
        var exists = await _repository.ExistsByNormalizedRegistrationAsync(normalizedCandidate, currentId?.Value);

        if (exists)
            throw new Exception("Ya existe una aeronave con esa matricula");
    }
}

