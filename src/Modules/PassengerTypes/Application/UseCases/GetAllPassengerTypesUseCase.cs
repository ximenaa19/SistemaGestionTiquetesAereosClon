// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PassengerTypes\Application\UseCases\GetAllPassengerTypesUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PassengerTypes.Domain.Aggregate;
using GestionAerolineas.src.Modules.PassengerTypes.Domain.Repositories;

namespace GestionAerolineas.src.Modules.PassengerTypes.Application.UseCases;

public class GetAllPassengerTypesUseCase
{
    private readonly IPassengerTypeRepository _repository;

    public GetAllPassengerTypesUseCase(IPassengerTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<PassengerType>> ExecuteAsync()
    {
        return await _repository.GetAllAsync();
    }
}

