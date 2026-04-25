// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\TicketStatuses\Application\UseCases\GetTicketStatusByIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.TicketStatuses.Domain.Aggregate;
using GestionAerolineas.src.Modules.TicketStatuses.Domain.Repositories;
using GestionAerolineas.src.Modules.TicketStatuses.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.TicketStatuses.Application.UseCases;

public class GetTicketStatusByIdUseCase
{
    private readonly ITicketStatusRepository _repository;

    public GetTicketStatusByIdUseCase(ITicketStatusRepository repository)
    {
        _repository = repository;
    }

    public async Task<TicketStatus?> ExecuteAsync(int id)
    {
        var idVO = TicketStatusId.Create(id);
        return await _repository.GetByIdAsync(idVO);
    }
}
