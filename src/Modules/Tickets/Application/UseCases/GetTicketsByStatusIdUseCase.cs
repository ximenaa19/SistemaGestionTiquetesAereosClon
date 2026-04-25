// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Tickets\Application\UseCases\GetTicketsByStatusIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Tickets.Domain.Aggregate;
using GestionAerolineas.src.Modules.Tickets.Domain.Repositories;
using GestionAerolineas.src.Modules.Tickets.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Tickets.Application.UseCases;

public class GetTicketsByStatusIdUseCase
{
    private readonly ITicketRepository _repository;

    public GetTicketsByStatusIdUseCase(ITicketRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Ticket>> ExecuteAsync(int statusId)
    {
        return _repository.GetByStatusIdAsync(TicketStatusId.Create(statusId));
    }
}

