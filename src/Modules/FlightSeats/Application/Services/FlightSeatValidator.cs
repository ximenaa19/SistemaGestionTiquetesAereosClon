// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightSeats\Application\Services\FlightSeatValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.CabinTypes.Domain.ValueObject;
using GestionAerolineas.src.Modules.CabinTypes.Infrastructure.Repository;
using GestionAerolineas.src.Modules.FlightSeats.Application.Interfaces;
using GestionAerolineas.src.Modules.FlightSeats.Domain.Repositories;
using GestionAerolineas.src.Modules.FlightSeats.Domain.ValueObject;
using GestionAerolineas.src.Modules.Flights.Domain.ValueObject;
using GestionAerolineas.src.Modules.Flights.Infrastructure.Repository;
using GestionAerolineas.src.Modules.SeatLocationTypes.Domain.ValueObject;
using GestionAerolineas.src.Modules.SeatLocationTypes.Infrastructure.Repository;

namespace GestionAerolineas.src.Modules.FlightSeats.Application.Services;

public class FlightSeatValidator : IFlightSeatValidator
{
    private readonly IFlightSeatRepository _repository;
    private readonly FlightRepository _flightRepository;
    private readonly CabinTypeRepository _cabinTypeRepository;
    private readonly SeatLocationTypeRepository _seatLocationTypeRepository;

    public FlightSeatValidator(
        IFlightSeatRepository repository,
        FlightRepository flightRepository,
        CabinTypeRepository cabinTypeRepository,
        SeatLocationTypeRepository seatLocationTypeRepository)
    {
        _repository = repository;
        _flightRepository = flightRepository;
        _cabinTypeRepository = cabinTypeRepository;
        _seatLocationTypeRepository = seatLocationTypeRepository;
    }

    public async Task ValidateFlightExistsAsync(FlightSeatFlightId flightId)
    {
        var exists = await _flightRepository.ExistsAsync(FlightId.Create(flightId.Value));
        if (!exists)
            throw new Exception("El vuelo no existe");
    }

    public async Task ValidateCabinTypeExistsAsync(FlightSeatCabinTypeId cabinTypeId)
    {
        var exists = await _cabinTypeRepository.ExistsAsync(CabinTypesId.Create(cabinTypeId.Value));
        if (!exists)
            throw new Exception("El tipo de cabina no existe");
    }

    public async Task ValidateLocationTypeExistsAsync(FlightSeatLocationTypeId locationTypeId)
    {
        var exists = await _seatLocationTypeRepository.ExistsAsync(SeatLocationTypeId.Create(locationTypeId.Value));
        if (!exists)
            throw new Exception("El tipo de ubicacion no existe");
    }

    public async Task ValidateUniqueSeatCodeInFlightAsync(FlightSeatFlightId flightId, FlightSeatCode code, FlightSeatId? currentId = null)
    {
        var normalized = FlightSeatCode.Normalize(code.Value);
        var exists = await _repository.ExistsByFlightAndNormalizedCodeAsync(flightId.Value, normalized, currentId?.Value);
        if (exists)
            throw new Exception("Ya existe un asiento con ese codigo para ese vuelo");
    }

    public async Task ValidateSeatCountWithinFlightCapacityAsync(FlightSeatFlightId flightId, FlightSeatId? currentId = null)
    {
        var flight = await _flightRepository.GetByIdAsync(FlightId.Create(flightId.Value));
        if (flight is null)
            throw new Exception("El vuelo no existe");

        var currentCount = await _repository.CountByFlightIdAsync(flightId.Value);
        var willAddNew = currentId is null || currentId.Value == 0;

        if (willAddNew && currentCount + 1 > flight.TotalCapacity.Value)
            throw new Exception("No se pueden crear mas asientos que la capacidad_total del vuelo");
    }
}
