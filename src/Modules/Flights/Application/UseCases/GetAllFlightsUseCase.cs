// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Flights\Application\UseCases\GetAllFlightsUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Flights.Domain.Aggregate;
using GestionAerolineas.src.Modules.Flights.Domain.Repositories;

namespace GestionAerolineas.src.Modules.Flights.Application.UseCases;

public class GetAllFlightsUseCase
{
    private readonly IFlightRepository _repository;

    public GetAllFlightsUseCase(IFlightRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Flight>> ExecuteAsync()
    {
        return _repository.GetAllAsync();
    }
}

