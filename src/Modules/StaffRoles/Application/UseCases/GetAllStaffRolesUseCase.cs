// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\StaffRoles\Application\UseCases\GetAllStaffRolesUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.StaffRoles.Domain.Aggregate;
using GestionAerolineas.src.Modules.StaffRoles.Domain.Repositories;

namespace GestionAerolineas.src.Modules.StaffRoles.Application.UseCases;

public class GetAllStaffRolesUseCase
{
    private readonly IStaffRoleRepository _repository;

    public GetAllStaffRolesUseCase(IStaffRoleRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<StaffRole>> ExecuteAsync()
    {
        return await _repository.GetAllAsync();
    }
}
