// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Permissions\Application\UseCases\GetPermissionByNameUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Permissions.Domain.Aggregate;
using GestionAerolineas.src.Modules.Permissions.Domain.Repositories;
using GestionAerolineas.src.Modules.Permissions.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Permissions.Application.UseCases;

public class GetPermissionByNameUseCase
{
    private readonly IPermissionRepository _repository;

    public GetPermissionByNameUseCase(IPermissionRepository repository)
    {
        _repository = repository;
    }

    public async Task<Permission?> ExecuteAsync(string name)
    {
        var nameVO = PermissionName.Create(name);
        return await _repository.GetByNameAsync(nameVO);
    }
}
