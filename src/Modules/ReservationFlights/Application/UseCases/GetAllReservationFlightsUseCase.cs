// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationFlights\Application\UseCases\GetAllReservationFlightsUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.ReservationFlights.Domain.Aggregate;
using GestionAerolineas.src.Modules.ReservationFlights.Domain.Repositories;

namespace GestionAerolineas.src.Modules.ReservationFlights.Application.UseCases;

public class GetAllReservationFlightsUseCase
{
    private readonly IReservationFlightRepository _repository;

    public GetAllReservationFlightsUseCase(IReservationFlightRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<ReservationFlight>> ExecuteAsync()
    {
        return _repository.GetAllAsync();
    }
}

