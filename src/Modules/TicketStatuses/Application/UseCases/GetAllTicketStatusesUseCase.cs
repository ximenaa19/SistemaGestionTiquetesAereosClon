// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\TicketStatuses\Application\UseCases\GetAllTicketStatusesUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.TicketStatuses.Domain.Aggregate;
using GestionAerolineas.src.Modules.TicketStatuses.Domain.Repositories;

namespace GestionAerolineas.src.Modules.TicketStatuses.Application.UseCases;

public class GetAllTicketStatusesUseCase
{
    private readonly ITicketStatusRepository _repository;

    public GetAllTicketStatusesUseCase(ITicketStatusRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<TicketStatus>> ExecuteAsync()
    {
        return await _repository.GetAllAsync();
    }
}
