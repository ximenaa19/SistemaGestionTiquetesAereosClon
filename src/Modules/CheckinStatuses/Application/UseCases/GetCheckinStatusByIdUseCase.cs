// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\CheckinStatuses\Application\UseCases\GetCheckinStatusByIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.CheckinStatuses.Domain.Aggregate;
using GestionAerolineas.src.Modules.CheckinStatuses.Domain.Repositories;
using GestionAerolineas.src.Modules.CheckinStatuses.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.CheckinStatuses.Application.UseCases;

public class GetCheckinStatusByIdUseCase
{
    private readonly ICheckinStatusRepository _repository;

    public GetCheckinStatusByIdUseCase(ICheckinStatusRepository repository)
    {
        _repository = repository;
    }

    public async Task<CheckinStatus?> ExecuteAsync(int id)
    {
        var idVO = CheckinStatusId.Create(id);
        return await _repository.GetByIdAsync(idVO);
    }
}
