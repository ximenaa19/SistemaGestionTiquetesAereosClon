// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\StaffRoles\Application\UseCases\DeleteStaffRoleUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.StaffRoles.Domain.Repositories;
using GestionAerolineas.src.Modules.StaffRoles.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.StaffRoles.Application.UseCases;

public class DeleteStaffRoleUseCase
{
    private readonly IStaffRoleRepository _repository;

    public DeleteStaffRoleUseCase(IStaffRoleRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var staffRoleId = StaffRoleId.Create(id);
        var staffRole = await _repository.GetByIdAsync(staffRoleId);

        if (staffRole is null)
            throw new KeyNotFoundException($"StaffRole con id '{staffRoleId.Value}' no existe.");

        await _repository.DeleteAsync(staffRole);
    }
}
