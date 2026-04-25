// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Checkins\Application\UseCases\GetCheckinsByStatusIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Checkins.Application.Interfaces;
using GestionAerolineas.src.Modules.Checkins.Domain.Aggregate;
using GestionAerolineas.src.Modules.Checkins.Domain.Repositories;
using GestionAerolineas.src.Modules.Checkins.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Checkins.Application.UseCases;

public class GetCheckinsByStatusIdUseCase
{
    private readonly ICheckinRepository _repository;
    private readonly ICheckinValidator _validator;

    public GetCheckinsByStatusIdUseCase(ICheckinRepository repository, ICheckinValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<IEnumerable<Checkin>> ExecuteAsync(int statusId)
    {
        var statusIdVO = CheckinStatusId.Create(statusId);
        await _validator.ValidateStatusExistsAsync(statusIdVO);
        return await _repository.GetByStatusIdAsync(statusIdVO);
    }
}
