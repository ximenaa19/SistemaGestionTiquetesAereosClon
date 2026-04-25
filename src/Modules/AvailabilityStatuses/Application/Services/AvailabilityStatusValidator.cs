// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\AvailabilityStatuses\Application\Services\AvailabilityStatusValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.AvailabilityStatuses.Application.Interfaces;
using GestionAerolineas.src.Modules.AvailabilityStatuses.Domain.Repositories;
using GestionAerolineas.src.Modules.AvailabilityStatuses.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.AvailabilityStatuses.Application.Services;

public class AvailabilityStatusValidator : IAvailabilityStatusValidator
{
    private readonly IAvailabilityStatusRepository _repository;

    public AvailabilityStatusValidator(IAvailabilityStatusRepository repository)
    {
        _repository = repository;
    }

    public async Task ValidateNameAsync(AvailabilityStatusName name, AvailabilityStatusId? currentId = null)
    {
        var existingByName = await _repository.GetByNameAsync(name);

        if (existingByName is not null && existingByName.Id != currentId)
            throw new Exception("Ya existe un estado de disponibilidad con ese nombre");
    }
}
