// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Tickets\Application\UseCases\GetTicketsByReservationCodeUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Tickets.Domain.Aggregate;
using GestionAerolineas.src.Modules.Tickets.Domain.Repositories;

namespace GestionAerolineas.src.Modules.Tickets.Application.UseCases;

public class GetTicketsByReservationCodeUseCase
{
    private readonly ITicketRepository _repository;

    public GetTicketsByReservationCodeUseCase(ITicketRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Ticket>> ExecuteAsync(string reservationCode)
    {
        return _repository.GetByReservationCodeAsync(reservationCode);
    }
}

