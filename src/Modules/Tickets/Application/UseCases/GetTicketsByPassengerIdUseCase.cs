// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Tickets\Application\UseCases\GetTicketsByPassengerIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Passengers.Domain.ValueObject;
using GestionAerolineas.src.Modules.Passengers.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Tickets.Domain.Aggregate;
using GestionAerolineas.src.Modules.Tickets.Domain.Repositories;

namespace GestionAerolineas.src.Modules.Tickets.Application.UseCases;

public class GetTicketsByPassengerIdUseCase
{
    private readonly ITicketRepository _ticketRepository;
    private readonly PassengerRepository _passengerRepository;

    public GetTicketsByPassengerIdUseCase(ITicketRepository ticketRepository, PassengerRepository passengerRepository)
    {
        _ticketRepository = ticketRepository;
        _passengerRepository = passengerRepository;
    }

    public async Task<IEnumerable<Ticket>> ExecuteAsync(int passengerId)
    {
        var exists = await _passengerRepository.ExistsAsync(PassengerId.Create(passengerId));
        if (!exists)
            throw new Exception("El pasajero no existe");

        return await _ticketRepository.GetByPassengerIdAsync(passengerId);
    }
}

