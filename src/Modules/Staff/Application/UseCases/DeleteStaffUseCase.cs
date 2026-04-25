// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Staff\Application\UseCases\DeleteStaffUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Staff.Domain.Repositories;
using GestionAerolineas.src.Modules.Staff.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Staff.Application.UseCases;

public class DeleteStaffUseCase
{
    private readonly IStaffRepository _repository;

    public DeleteStaffUseCase(IStaffRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(StaffId.Create(id));
        if (entity is null)
            return;

        await _repository.DeleteAsync(entity);
    }
}

