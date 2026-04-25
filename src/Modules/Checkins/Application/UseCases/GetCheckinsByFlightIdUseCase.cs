// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Checkins\Application\UseCases\GetCheckinsByFlightIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Checkins.Domain.Aggregate;
using GestionAerolineas.src.Modules.Checkins.Domain.Repositories;
using GestionAerolineas.src.Modules.Flights.Domain.ValueObject;
using GestionAerolineas.src.Modules.Flights.Infrastructure.Repository;

namespace GestionAerolineas.src.Modules.Checkins.Application.UseCases;

public class GetCheckinsByFlightIdUseCase
{
    private readonly ICheckinRepository _repository;
    private readonly FlightRepository _flightRepository;

    public GetCheckinsByFlightIdUseCase(ICheckinRepository repository, FlightRepository flightRepository)
    {
        _repository = repository;
        _flightRepository = flightRepository;
    }

    public async Task<IEnumerable<Checkin>> ExecuteAsync(int flightId)
    {
        var exists = await _flightRepository.ExistsAsync(FlightId.Create(flightId));
        if (!exists)
            throw new Exception("El vuelo no existe");

        return await _repository.GetByFlightIdAsync(flightId);
    }
}

