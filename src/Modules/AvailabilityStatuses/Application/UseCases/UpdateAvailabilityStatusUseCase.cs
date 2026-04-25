// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\AvailabilityStatuses\Application\UseCases\UpdateAvailabilityStatusUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.AvailabilityStatuses.Application.Interfaces;
using GestionAerolineas.src.Modules.AvailabilityStatuses.Domain.Aggregate;
using GestionAerolineas.src.Modules.AvailabilityStatuses.Domain.Repositories;
using GestionAerolineas.src.Modules.AvailabilityStatuses.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.AvailabilityStatuses.Application.UseCases;

public class UpdateAvailabilityStatusUseCase
{
    private readonly IAvailabilityStatusRepository _repository;
    private readonly IAvailabilityStatusValidator _validator;

    public UpdateAvailabilityStatusUseCase(
        IAvailabilityStatusRepository repository,
        IAvailabilityStatusValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(int id, string name)
    {
        var idVO = AvailabilityStatusId.Create(id);

        var existing = await _repository.GetByIdAsync(idVO);

        if (existing is null)
            throw new Exception("El estado de disponibilidad no existe");

        var nameVO = AvailabilityStatusName.Create(name);

        await _validator.ValidateNameAsync(nameVO, idVO);

        var updated = AvailabilityStatus.Create(idVO, nameVO);

        await _repository.UpdateAsync(updated);
    }
}
