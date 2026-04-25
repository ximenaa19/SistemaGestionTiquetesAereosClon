// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Airports\Application\UseCases\GetAirportByIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Airports.Domain.Aggregate;
using GestionAerolineas.src.Modules.Airports.Domain.Repositories;
using GestionAerolineas.src.Modules.Airports.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Airports.Application.UseCases;

public class GetAirportByIdUseCase
{
    private readonly IAirportRepository _repository;

    public GetAirportByIdUseCase(IAirportRepository repository)
    {
        _repository = repository;
    }

    public Task<Airport?> ExecuteAsync(int id)
    {
        return _repository.GetByIdAsync(AirportId.Create(id));
    }
}
