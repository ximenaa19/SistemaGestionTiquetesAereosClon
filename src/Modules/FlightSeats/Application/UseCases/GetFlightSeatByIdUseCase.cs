// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightSeats\Application\UseCases\GetFlightSeatByIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.FlightSeats.Domain.Aggregate;
using GestionAerolineas.src.Modules.FlightSeats.Domain.Repositories;
using GestionAerolineas.src.Modules.FlightSeats.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.FlightSeats.Application.UseCases;

public class GetFlightSeatByIdUseCase
{
    private readonly IFlightSeatRepository _repository;

    public GetFlightSeatByIdUseCase(IFlightSeatRepository repository)
    {
        _repository = repository;
    }

    public Task<FlightSeat?> ExecuteAsync(int id)
    {
        return _repository.GetByIdAsync(FlightSeatId.Create(id));
    }
}

