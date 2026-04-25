// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightSeats\Application\UseCases\GetAllFlightSeatsUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.FlightSeats.Domain.Aggregate;
using GestionAerolineas.src.Modules.FlightSeats.Domain.Repositories;

namespace GestionAerolineas.src.Modules.FlightSeats.Application.UseCases;

public class GetAllFlightSeatsUseCase
{
    private readonly IFlightSeatRepository _repository;

    public GetAllFlightSeatsUseCase(IFlightSeatRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<FlightSeat>> ExecuteAsync()
    {
        return _repository.GetAllAsync();
    }
}

