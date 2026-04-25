// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Permissions\Application\UseCases\DeletePermissionUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Permissions.Domain.Repositories;
using GestionAerolineas.src.Modules.Permissions.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Permissions.Application.UseCases;

public class DeletePermissionUseCase
{
    private readonly IPermissionRepository _repository;

    public DeletePermissionUseCase(IPermissionRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var permissionId = PermissionId.Create(id);
        var permission = await _repository.GetByIdAsync(permissionId);

        if (permission is null)
            throw new KeyNotFoundException($"Permission con id '{permissionId.Value}' no existe.");

        await _repository.DeleteAsync(permission);
    }
}
