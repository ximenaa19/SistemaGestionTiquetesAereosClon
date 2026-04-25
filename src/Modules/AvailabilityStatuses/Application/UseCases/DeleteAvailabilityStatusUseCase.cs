// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\AvailabilityStatuses\Application\UseCases\DeleteAvailabilityStatusUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.AvailabilityStatuses.Domain.Repositories;
using GestionAerolineas.src.Modules.AvailabilityStatuses.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.AvailabilityStatuses.Application.UseCases;

public class DeleteAvailabilityStatusUseCase
{
    private readonly IAvailabilityStatusRepository _repository;

    public DeleteAvailabilityStatusUseCase(IAvailabilityStatusRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var availabilityStatusId = AvailabilityStatusId.Create(id);
        var availabilityStatus = await _repository.GetByIdAsync(availabilityStatusId);

        if (availabilityStatus is null)
            throw new KeyNotFoundException($"AvailabilityStatus con id '{availabilityStatusId.Value}' no existe.");

        await _repository.DeleteAsync(availabilityStatus);
    }
}
