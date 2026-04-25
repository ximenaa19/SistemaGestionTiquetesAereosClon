// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\CheckinStatuses\Application\Services\CheckinStatusValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.CheckinStatuses.Application.Interfaces;
using GestionAerolineas.src.Modules.CheckinStatuses.Domain.Repositories;
using GestionAerolineas.src.Modules.CheckinStatuses.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.CheckinStatuses.Application.Services;

public class CheckinStatusValidator : ICheckinStatusValidator
{
    private readonly ICheckinStatusRepository _repository;

    public CheckinStatusValidator(ICheckinStatusRepository repository)
    {
        _repository = repository;
    }

    public async Task ValidateNameAsync(CheckinStatusName name, CheckinStatusId? currentId = null)
    {
        var existingByName = await _repository.GetByNameAsync(name);

        if (existingByName is not null && existingByName.Id != currentId)
            throw new Exception("Ya existe un estado de checkin con ese nombre");
    }
}
