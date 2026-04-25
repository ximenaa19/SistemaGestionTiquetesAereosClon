// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\CheckinStatuses\Application\UseCases\GetAllCheckinStatusesUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.CheckinStatuses.Domain.Aggregate;
using GestionAerolineas.src.Modules.CheckinStatuses.Domain.Repositories;

namespace GestionAerolineas.src.Modules.CheckinStatuses.Application.UseCases;

public class GetAllCheckinStatusesUseCase
{
    private readonly ICheckinStatusRepository _repository;

    public GetAllCheckinStatusesUseCase(ICheckinStatusRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CheckinStatus>> ExecuteAsync()
    {
        return await _repository.GetAllAsync();
    }
}
