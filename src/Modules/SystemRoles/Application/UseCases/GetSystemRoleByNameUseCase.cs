// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\SystemRoles\Application\UseCases\GetSystemRoleByNameUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.SystemRoles.Domain.Aggregate;
using GestionAerolineas.src.Modules.SystemRoles.Domain.Repositories;
using GestionAerolineas.src.Modules.SystemRoles.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.SystemRoles.Application.UseCases;

public class GetSystemRoleByNameUseCase
{
    private readonly ISystemRoleRepository _repository;

    public GetSystemRoleByNameUseCase(ISystemRoleRepository repository)
    {
        _repository = repository;
    }

    public async Task<SystemRole?> ExecuteAsync(string name)
    {
        var nameVO = SystemRoleName.Create(name);
        return await _repository.GetByNameAsync(nameVO);
    }
}
