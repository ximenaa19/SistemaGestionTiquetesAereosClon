// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\CheckinStatuses\Application\UseCases\CreateCheckinStatusUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.CheckinStatuses.Application.Interfaces;
using GestionAerolineas.src.Modules.CheckinStatuses.Domain.Aggregate;
using GestionAerolineas.src.Modules.CheckinStatuses.Domain.Repositories;
using GestionAerolineas.src.Modules.CheckinStatuses.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.CheckinStatuses.Application.UseCases;

public class CreateCheckinStatusUseCase
{
    private readonly ICheckinStatusRepository _repository;
    private readonly ICheckinStatusValidator _validator;

    public CreateCheckinStatusUseCase(
        ICheckinStatusRepository repository,
        ICheckinStatusValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(string name)
    {
        var nameVO = CheckinStatusName.Create(name);

        await _validator.ValidateNameAsync(nameVO);

        var entity = CheckinStatus.CreateNew(nameVO);

        await _repository.AddAsync(entity);
    }
}
