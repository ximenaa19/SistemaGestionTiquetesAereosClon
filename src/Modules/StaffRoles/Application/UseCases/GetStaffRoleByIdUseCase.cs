// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\StaffRoles\Application\UseCases\GetStaffRoleByIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.StaffRoles.Domain.Aggregate;
using GestionAerolineas.src.Modules.StaffRoles.Domain.Repositories;
using GestionAerolineas.src.Modules.StaffRoles.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.StaffRoles.Application.UseCases;

public class GetStaffRoleByIdUseCase
{
    private readonly IStaffRoleRepository _repository;

    public GetStaffRoleByIdUseCase(IStaffRoleRepository repository)
    {
        _repository = repository;
    }

    public async Task<StaffRole?> ExecuteAsync(int id)
    {
        var idVO = StaffRoleId.Create(id);
        return await _repository.GetByIdAsync(idVO);
    }
}
