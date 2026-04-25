// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\StaffAvailability\Application\UseCases\GetAllStaffAvailabilityUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.StaffAvailability.Domain.Aggregate;
using GestionAerolineas.src.Modules.StaffAvailability.Domain.Repositories;

namespace GestionAerolineas.src.Modules.StaffAvailability.Application.UseCases;

public class GetAllStaffAvailabilityUseCase
{
    private readonly IStaffAvailabilityRepository _repository;

    public GetAllStaffAvailabilityUseCase(IStaffAvailabilityRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<StaffAvailabilityBlock>> ExecuteAsync()
    {
        return _repository.GetAllAsync();
    }
}
