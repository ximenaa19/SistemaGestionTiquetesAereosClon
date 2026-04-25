// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\SystemRoles\Application\UseCases\GetSystemRoleByIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.SystemRoles.Domain.Aggregate;
using GestionAerolineas.src.Modules.SystemRoles.Domain.Repositories;
using GestionAerolineas.src.Modules.SystemRoles.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.SystemRoles.Application.UseCases;

public class GetSystemRoleByIdUseCase
{
    private readonly ISystemRoleRepository _repository;

    public GetSystemRoleByIdUseCase(ISystemRoleRepository repository)
    {
        _repository = repository;
    }

    public async Task<SystemRole?> ExecuteAsync(int id)
    {
        var idVO = SystemRoleId.Create(id);
        return await _repository.GetByIdAsync(idVO);
    }
}
