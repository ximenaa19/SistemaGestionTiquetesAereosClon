// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\SystemRoles\Application\UseCases\DeleteSystemRoleUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.SystemRoles.Domain.Repositories;
using GestionAerolineas.src.Modules.SystemRoles.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.SystemRoles.Application.UseCases;

public class DeleteSystemRoleUseCase
{
    private readonly ISystemRoleRepository _repository;

    public DeleteSystemRoleUseCase(ISystemRoleRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var systemRoleId = SystemRoleId.Create(id);
        var systemRole = await _repository.GetByIdAsync(systemRoleId);

        if (systemRole is null)
            throw new KeyNotFoundException($"SystemRole con id '{systemRoleId.Value}' no existe.");

        await _repository.DeleteAsync(systemRole);
    }
}
