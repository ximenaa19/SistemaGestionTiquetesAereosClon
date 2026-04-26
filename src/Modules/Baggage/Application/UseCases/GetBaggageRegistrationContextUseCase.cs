using GestionAerolineas.src.Modules.Baggage.Domain.Models;
using GestionAerolineas.src.Modules.Baggage.Domain.Repositories;

namespace GestionAerolineas.src.Modules.Baggage.Application.UseCases;

public class GetBaggageRegistrationContextUseCase
{
    private readonly IBaggageRecordRepository _repository;

    public GetBaggageRegistrationContextUseCase(IBaggageRecordRepository repository)
    {
        _repository = repository;
    }

    public async Task<BaggageRegistrationContext> ExecuteByTicketAsync(int ticketId)
    {
        if (ticketId <= 0)
            throw new ArgumentException("El tiquete es obligatorio.");

        var context = await _repository.GetRegistrationContextByTicketIdAsync(ticketId);
        if (context is null)
            throw new InvalidOperationException("No se encontro el tiquete o no esta relacionado con una reserva.");

        return context;
    }

    public async Task<BaggageRegistrationContext> ExecuteByReservationAsync(int reservationId)
    {
        if (reservationId <= 0)
            throw new ArgumentException("La reserva es obligatoria.");

        var context = await _repository.GetRegistrationContextByReservationIdAsync(reservationId);
        if (context is null)
            throw new InvalidOperationException("No se encontro la reserva.");

        return context;
    }
}
