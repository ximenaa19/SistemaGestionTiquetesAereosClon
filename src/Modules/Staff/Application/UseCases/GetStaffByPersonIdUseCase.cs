// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Staff\Application\UseCases\GetStaffByPersonIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Staff.Domain.Aggregate;
using GestionAerolineas.src.Modules.Staff.Domain.Repositories;
using GestionAerolineas.src.Modules.Staff.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Staff.Application.UseCases;

public class GetStaffByPersonIdUseCase
{
    private readonly IStaffRepository _repository;

    public GetStaffByPersonIdUseCase(IStaffRepository repository)
    {
        _repository = repository;
    }

    public Task<StaffMember?> ExecuteAsync(int personId)
    {
        return _repository.GetByPersonIdAsync(StaffPersonId.Create(personId));
    }
}

