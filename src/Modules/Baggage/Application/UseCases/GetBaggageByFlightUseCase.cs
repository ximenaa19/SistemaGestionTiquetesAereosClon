using GestionAerolineas.src.Modules.Baggage.Domain.Models;
using GestionAerolineas.src.Modules.Baggage.Domain.Repositories;

namespace GestionAerolineas.src.Modules.Baggage.Application.UseCases;

public class GetBaggageByFlightUseCase
{
    private readonly IBaggageRecordRepository _repository;

    public GetBaggageByFlightUseCase(IBaggageRecordRepository repository)
    {
        _repository = repository;
    }

    public Task<IReadOnlyList<BaggageRecordView>> ExecuteAsync(int flightId)
    {
        if (flightId <= 0)
            throw new ArgumentException("El vuelo es obligatorio.");

        return _repository.GetByFlightIdAsync(flightId);
    }
}
