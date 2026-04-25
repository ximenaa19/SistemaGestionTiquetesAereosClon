// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\StaffAvailability\Application\UseCases\DeleteStaffAvailabilityUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.StaffAvailability.Domain.Repositories;
using GestionAerolineas.src.Modules.StaffAvailability.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.StaffAvailability.Application.UseCases;

public class DeleteStaffAvailabilityUseCase
{
    private readonly IStaffAvailabilityRepository _repository;

    public DeleteStaffAvailabilityUseCase(IStaffAvailabilityRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(StaffAvailabilityId.Create(id));
        if (entity is null)
            return;

        await _repository.DeleteAsync(entity);
    }
}

