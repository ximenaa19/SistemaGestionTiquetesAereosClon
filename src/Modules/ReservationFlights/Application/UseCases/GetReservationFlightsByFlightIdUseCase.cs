// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationFlights\Application\UseCases\GetReservationFlightsByFlightIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.ReservationFlights.Domain.Aggregate;
using GestionAerolineas.src.Modules.ReservationFlights.Domain.Repositories;
using GestionAerolineas.src.Modules.ReservationFlights.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.ReservationFlights.Application.UseCases;

public class GetReservationFlightsByFlightIdUseCase
{
    private readonly IReservationFlightRepository _repository;

    public GetReservationFlightsByFlightIdUseCase(IReservationFlightRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<ReservationFlight>> ExecuteAsync(int flightId)
    {
        return _repository.GetByFlightIdAsync(ReservationFlightFlightId.Create(flightId));
    }
}

