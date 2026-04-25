// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\StaffRoles\Application\Services\StaffRoleValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.StaffRoles.Application.Interfaces;
using GestionAerolineas.src.Modules.StaffRoles.Domain.Repositories;
using GestionAerolineas.src.Modules.StaffRoles.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.StaffRoles.Application.Services;

public class StaffRoleValidator : IStaffRoleValidator
{
    private readonly IStaffRoleRepository _repository;

    public StaffRoleValidator(IStaffRoleRepository repository)
    {
        _repository = repository;
    }

    public async Task ValidateNameAsync(StaffRoleName name, StaffRoleId? currentId = null)
    {
        var existingByName = await _repository.GetByNameAsync(name);

        if (existingByName is not null && existingByName.Id != currentId)
            throw new Exception("Ya existe un cargo del personal con ese nombre");
    }
}
