// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PassengerTypes\Application\Services\PassengerTypeValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PassengerTypes.Application.Interfaces;
using GestionAerolineas.src.Modules.PassengerTypes.Domain.Repositories;
using GestionAerolineas.src.Modules.PassengerTypes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.PassengerTypes.Application.Services;

public class PassengerTypeValidator : IPassengerTypeValidator
{
    private readonly IPassengerTypeRepository _repository;

    public PassengerTypeValidator(IPassengerTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task ValidateNameAsync(PassengerTypeName name, PassengerTypeId? currentId = null)
    {
        var existingByName = await _repository.GetByNameAsync(name);

        if (existingByName is not null && existingByName.Id != currentId)
            throw new Exception("Ya existe un tipo de pasajero con ese nombre");
    }
}
