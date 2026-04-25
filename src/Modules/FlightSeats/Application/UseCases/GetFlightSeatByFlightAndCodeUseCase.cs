// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightSeats\Application\UseCases\GetFlightSeatByFlightAndCodeUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.FlightSeats.Domain.Aggregate;
using GestionAerolineas.src.Modules.FlightSeats.Domain.Repositories;
using GestionAerolineas.src.Modules.FlightSeats.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.FlightSeats.Application.UseCases;

public class GetFlightSeatByFlightAndCodeUseCase
{
    private readonly IFlightSeatRepository _repository;

    public GetFlightSeatByFlightAndCodeUseCase(IFlightSeatRepository repository)
    {
        _repository = repository;
    }

    public Task<FlightSeat?> ExecuteAsync(int flightId, string seatCode)
    {
        return _repository.GetByFlightAndCodeAsync(FlightSeatFlightId.Create(flightId), FlightSeatCode.Create(seatCode));
    }
}

