// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\StaffRoles\Application\UseCases\UpdateStaffRoleUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.StaffRoles.Application.Interfaces;
using GestionAerolineas.src.Modules.StaffRoles.Domain.Aggregate;
using GestionAerolineas.src.Modules.StaffRoles.Domain.Repositories;
using GestionAerolineas.src.Modules.StaffRoles.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.StaffRoles.Application.UseCases;

public class UpdateStaffRoleUseCase
{
    private readonly IStaffRoleRepository _repository;
    private readonly IStaffRoleValidator _validator;

    public UpdateStaffRoleUseCase(
        IStaffRoleRepository repository,
        IStaffRoleValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(int id, string name)
    {
        var idVO = StaffRoleId.Create(id);

        var existing = await _repository.GetByIdAsync(idVO);

        if (existing is null)
            throw new Exception("El cargo del personal no existe");

        var nameVO = StaffRoleName.Create(name);

        await _validator.ValidateNameAsync(nameVO, idVO);

        var updated = StaffRole.Create(idVO, nameVO);

        await _repository.UpdateAsync(updated);
    }
}
