// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PassengerTypes\Application\UseCases\GetPassengerTypeByIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PassengerTypes.Domain.Aggregate;
using GestionAerolineas.src.Modules.PassengerTypes.Domain.Repositories;
using GestionAerolineas.src.Modules.PassengerTypes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.PassengerTypes.Application.UseCases;

public class GetPassengerTypeByIdUseCase
{
    private readonly IPassengerTypeRepository _repository;

    public GetPassengerTypeByIdUseCase(IPassengerTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<PassengerType?> ExecuteAsync(int id)
    {
        var idVO = PassengerTypeId.Create(id);
        return await _repository.GetByIdAsync(idVO);
    }
}

