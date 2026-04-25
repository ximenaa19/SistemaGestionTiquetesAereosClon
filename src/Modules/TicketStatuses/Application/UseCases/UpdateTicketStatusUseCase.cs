// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\TicketStatuses\Application\UseCases\UpdateTicketStatusUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.TicketStatuses.Application.Interfaces;
using GestionAerolineas.src.Modules.TicketStatuses.Domain.Aggregate;
using GestionAerolineas.src.Modules.TicketStatuses.Domain.Repositories;
using GestionAerolineas.src.Modules.TicketStatuses.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.TicketStatuses.Application.UseCases;

public class UpdateTicketStatusUseCase
{
    private readonly ITicketStatusRepository _repository;
    private readonly ITicketStatusValidator _validator;

    public UpdateTicketStatusUseCase(
        ITicketStatusRepository repository,
        ITicketStatusValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(int id, string name)
    {
        var idVO = TicketStatusId.Create(id);

        var existing = await _repository.GetByIdAsync(idVO);

        if (existing is null)
            throw new Exception("El estado de tiquete no existe");

        var nameVO = TicketStatusName.Create(name);

        await _validator.ValidateNameAsync(nameVO, idVO);

        var updated = TicketStatus.Create(idVO, nameVO);

        await _repository.UpdateAsync(updated);
    }
}
