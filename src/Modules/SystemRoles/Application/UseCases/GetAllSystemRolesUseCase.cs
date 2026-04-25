// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\SystemRoles\Application\UseCases\GetAllSystemRolesUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.SystemRoles.Domain.Aggregate;
using GestionAerolineas.src.Modules.SystemRoles.Domain.Repositories;

namespace GestionAerolineas.src.Modules.SystemRoles.Application.UseCases;

public class GetAllSystemRolesUseCase
{
    private readonly ISystemRoleRepository _repository;

    public GetAllSystemRolesUseCase(ISystemRoleRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<SystemRole>> ExecuteAsync()
    {
        return await _repository.GetAllAsync();
    }
}
