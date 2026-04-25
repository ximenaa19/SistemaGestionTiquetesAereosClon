// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Airports\Application\UseCases\GetAllAirportsUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Airports.Domain.Aggregate;
using GestionAerolineas.src.Modules.Airports.Domain.Repositories;

namespace GestionAerolineas.src.Modules.Airports.Application.UseCases;

public class GetAllAirportsUseCase
{
    private readonly IAirportRepository _repository;

    public GetAllAirportsUseCase(IAirportRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Airport>> ExecuteAsync()
    {
        return _repository.GetAllAsync();
    }
}
