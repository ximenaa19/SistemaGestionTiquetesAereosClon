// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Permissions\Application\UseCases\GetAllPermissionsUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Permissions.Domain.Aggregate;
using GestionAerolineas.src.Modules.Permissions.Domain.Repositories;

namespace GestionAerolineas.src.Modules.Permissions.Application.UseCases;

public class GetAllPermissionsUseCase
{
    private readonly IPermissionRepository _repository;

    public GetAllPermissionsUseCase(IPermissionRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Permission>> ExecuteAsync()
    {
        return await _repository.GetAllAsync();
    }
}
