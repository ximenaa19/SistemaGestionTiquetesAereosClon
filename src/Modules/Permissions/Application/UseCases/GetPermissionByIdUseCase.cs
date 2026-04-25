// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Permissions\Application\UseCases\GetPermissionByIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Permissions.Domain.Aggregate;
using GestionAerolineas.src.Modules.Permissions.Domain.Repositories;
using GestionAerolineas.src.Modules.Permissions.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Permissions.Application.UseCases;

public class GetPermissionByIdUseCase
{
    private readonly IPermissionRepository _repository;

    public GetPermissionByIdUseCase(IPermissionRepository repository)
    {
        _repository = repository;
    }

    public async Task<Permission?> ExecuteAsync(int id)
    {
        var idVO = PermissionId.Create(id);
        return await _repository.GetByIdAsync(idVO);
    }
}
