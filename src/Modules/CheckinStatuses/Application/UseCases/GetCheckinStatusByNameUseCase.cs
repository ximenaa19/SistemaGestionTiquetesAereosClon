// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\CheckinStatuses\Application\UseCases\GetCheckinStatusByNameUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.CheckinStatuses.Domain.Aggregate;
using GestionAerolineas.src.Modules.CheckinStatuses.Domain.Repositories;
using GestionAerolineas.src.Modules.CheckinStatuses.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.CheckinStatuses.Application.UseCases;

public class GetCheckinStatusByNameUseCase
{
    private readonly ICheckinStatusRepository _repository;

    public GetCheckinStatusByNameUseCase(ICheckinStatusRepository repository)
    {
        _repository = repository;
    }

    public async Task<CheckinStatus?> ExecuteAsync(string name)
    {
        var nameVO = CheckinStatusName.Create(name);
        return await _repository.GetByNameAsync(nameVO);
    }
}
