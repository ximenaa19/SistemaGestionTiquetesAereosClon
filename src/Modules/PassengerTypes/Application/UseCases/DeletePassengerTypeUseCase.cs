// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PassengerTypes\Application\UseCases\DeletePassengerTypeUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PassengerTypes.Domain.Repositories;
using GestionAerolineas.src.Modules.PassengerTypes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.PassengerTypes.Application.UseCases;

public class DeletePassengerTypeUseCase
{
    private readonly IPassengerTypeRepository _repository;

    public DeletePassengerTypeUseCase(IPassengerTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var passengerTypeId = PassengerTypeId.Create(id);
        var passengerType = await _repository.GetByIdAsync(passengerTypeId);

        if (passengerType is null)
            throw new KeyNotFoundException($"PassengerType con id '{passengerTypeId.Value}' no existe.");

        await _repository.DeleteAsync(passengerType);
    }
}

