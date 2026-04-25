// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\SeatLocationTypes\Application\UseCases\DeleteSeatLocationTypeUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.SeatLocationTypes.Domain.Repositories;
using GestionAerolineas.src.Modules.SeatLocationTypes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.SeatLocationTypes.Application.UseCases;

public class DeleteSeatLocationTypeUseCase
{
    private readonly ISeatLocationTypeRepository _repository;

    public DeleteSeatLocationTypeUseCase(ISeatLocationTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var seatLocationTypeId = SeatLocationTypeId.Create(id);
        var seatLocationType = await _repository.GetByIdAsync(seatLocationTypeId);

        if (seatLocationType is null)
        {
            throw new KeyNotFoundException($"SeatLocationType con id '{seatLocationTypeId.Value}' no existe.");
        }

        await _repository.DeleteAsync(seatLocationType);
    }
}

