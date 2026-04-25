// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Staff\Application\UseCases\SearchStaffByPersonNameOrLastNameUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Staff.Domain.Aggregate;
using GestionAerolineas.src.Modules.Staff.Domain.Repositories;

namespace GestionAerolineas.src.Modules.Staff.Application.UseCases;

public class SearchStaffByPersonNameOrLastNameUseCase
{
    private readonly IStaffRepository _repository;

    public SearchStaffByPersonNameOrLastNameUseCase(IStaffRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<StaffMember>> ExecuteAsync(string searchText)
    {
        return _repository.SearchByPersonNameOrLastNameAsync(searchText);
    }
}

