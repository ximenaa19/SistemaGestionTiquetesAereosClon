// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Passengers\Application\UseCases\GetAllPassengersUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Passengers.Domain.Aggregate;
using GestionAerolineas.src.Modules.Passengers.Domain.Repositories;

namespace GestionAerolineas.src.Modules.Passengers.Application.UseCases;

public class GetAllPassengersUseCase
{
    private readonly IPassengerRepository _repository;

    public GetAllPassengersUseCase(IPassengerRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Passenger>> ExecuteAsync()
    {
        return _repository.GetAllAsync();
    }
}
