// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\AirportAirline\Application\Services\AirportAirlineValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.AirportAirline.Application.Interfaces;
using GestionAerolineas.src.Modules.AirportAirline.Domain.Repositories;
using GestionAerolineas.src.Modules.AirportAirline.Domain.ValueObject;
using GestionAerolineas.src.Modules.Airlines.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Airports.Infrastructure.Repository;

namespace GestionAerolineas.src.Modules.AirportAirline.Application.Services;

public class AirportAirlineValidator : IAirportAirlineValidator
{
    private readonly IAirportAirlineRepository _repository;
    private readonly AirportRepository _airportRepository;
    private readonly AirlineRepository _airlineRepository;

    public AirportAirlineValidator(
        IAirportAirlineRepository repository,
        AirportRepository airportRepository,
        AirlineRepository airlineRepository)
    {
        _repository = repository;
        _airportRepository = airportRepository;
        _airlineRepository = airlineRepository;
    }

    public async Task ValidateAirportExistsAsync(AirportAirlineAirportId airportId)
    {
        var exists = await _airportRepository.ExistsAsync(
            GestionAerolineas.src.Modules.Airports.Domain.ValueObject.AirportId.Create(airportId.Value));

        if (!exists)
            throw new Exception("El aeropuerto no existe");
    }

    public async Task ValidateAirlineExistsAsync(AirportAirlineAirlineId airlineId)
    {
        var exists = await _airlineRepository.ExistsAsync(
            GestionAerolineas.src.Modules.Airlines.Domain.ValueObject.AirlineId.Create(airlineId.Value));

        if (!exists)
            throw new Exception("La aerolinea no existe");
    }

    public async Task ValidateUniquePairAsync(
        AirportAirlineAirportId airportId,
        AirportAirlineAirlineId airlineId,
        AirportAirlineId? currentId = null)
    {
        var exists = await _repository.ExistsByAirportAndAirlineAsync(airportId.Value, airlineId.Value, currentId?.Value);
        if (exists)
            throw new Exception("Ya existe una relacion airport-airline para ese aeropuerto y aerolinea");
    }

    public Task ValidateDatesAsync(AirportAirlineStartDate startDate, AirportAirlineEndDate endDate)
    {
        if (endDate.Value.HasValue && endDate.Value.Value.Date < startDate.Value.Date)
            throw new Exception("La fecha_fin no puede ser menor que la fecha_inicio");

        return Task.CompletedTask;
    }
}

