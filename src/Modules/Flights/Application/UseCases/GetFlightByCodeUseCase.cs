// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Flights\Application\UseCases\GetFlightByCodeUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Flights.Domain.Aggregate;
using GestionAerolineas.src.Modules.Flights.Domain.Repositories;
using GestionAerolineas.src.Modules.Flights.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Flights.Application.UseCases;

public class GetFlightByCodeUseCase
{
    private readonly IFlightRepository _repository;

    public GetFlightByCodeUseCase(IFlightRepository repository)
    {
        _repository = repository;
    }

    public Task<Flight?> ExecuteAsync(string code)
    {
        return _repository.GetByCodeAsync(FlightCode.Create(code));
    }
}

