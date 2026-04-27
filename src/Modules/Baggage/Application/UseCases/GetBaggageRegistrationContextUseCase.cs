using GestionAerolineas.src.Modules.Baggage.Domain.Models;
using GestionAerolineas.src.Modules.Baggage.Domain.Repositories;

namespace GestionAerolineas.src.Modules.Baggage.Application.UseCases;

// caso de uso para obtener los datos de contexto antes de registrar equipaje
public class GetBaggageRegistrationContextUseCase
{
    // repositorio que sabe buscar reserva, tiquete, vuelo y pasajero relacionados
    private readonly IBaggageRecordRepository _repository;

    public GetBaggageRegistrationContextUseCase(IBaggageRecordRepository repository)
    {
        // inyecta la dependencia de persistencia
        _repository = repository;
    }

    public async Task<BaggageRegistrationContext> ExecuteByTicketAsync(int ticketId)
    {
        // valida que el tiquete sea obligatorio para poder buscar el contexto
        if (ticketId <= 0)
            throw new ArgumentException("El tiquete es obligatorio.");

        // consulta los datos relacionados al tiquete: reserva, vuelo, pasajero y total actual
        var context = await _repository.GetRegistrationContextByTicketIdAsync(ticketId);
        if (context is null)
            throw new InvalidOperationException("No se encontro el tiquete o no esta relacionado con una reserva.");

        // retorna el contexto para que el menu lo muestre antes de pedir el equipaje
        return context;
    }

    public async Task<BaggageRegistrationContext> ExecuteByReservationAsync(int reservationId)
    {
        // valida que la reserva exista como dato de entrada
        if (reservationId <= 0)
            throw new ArgumentException("La reserva es obligatoria.");

        // consulta los datos disponibles de la reserva y sus relaciones
        var context = await _repository.GetRegistrationContextByReservationIdAsync(reservationId);
        if (context is null)
            throw new InvalidOperationException("No se encontro la reserva.");

        // devuelve informacion util para confirmar que se esta registrando en la reserva correcta
        return context;
    }
}
