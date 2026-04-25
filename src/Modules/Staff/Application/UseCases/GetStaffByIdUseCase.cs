// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Staff\Application\UseCases\GetStaffByIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Staff.Domain.Aggregate;
using GestionAerolineas.src.Modules.Staff.Domain.Repositories;
using GestionAerolineas.src.Modules.Staff.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Staff.Application.UseCases;

public class GetStaffByIdUseCase
{
    private readonly IStaffRepository _repository;

    public GetStaffByIdUseCase(IStaffRepository repository)
    {
        _repository = repository;
    }

    public Task<StaffMember?> ExecuteAsync(int id)
    {
        return _repository.GetByIdAsync(StaffId.Create(id));
    }
}

