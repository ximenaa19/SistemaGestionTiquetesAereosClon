// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PassengerTypes\Application\UseCases\GetPassengerTypeByNameUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PassengerTypes.Domain.Aggregate;
using GestionAerolineas.src.Modules.PassengerTypes.Domain.Repositories;
using GestionAerolineas.src.Modules.PassengerTypes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.PassengerTypes.Application.UseCases;

public class GetPassengerTypeByNameUseCase
{
    private readonly IPassengerTypeRepository _repository;

    public GetPassengerTypeByNameUseCase(IPassengerTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<PassengerType?> ExecuteAsync(string name)
    {
        var nameVO = PassengerTypeName.Create(name);
        return await _repository.GetByNameAsync(nameVO);
    }
}

