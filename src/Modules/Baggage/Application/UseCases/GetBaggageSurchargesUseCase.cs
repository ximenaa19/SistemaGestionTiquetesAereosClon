using GestionAerolineas.src.Modules.Baggage.Domain.Models;
using GestionAerolineas.src.Modules.Baggage.Domain.Repositories;

namespace GestionAerolineas.src.Modules.Baggage.Application.UseCases;

public class GetBaggageSurchargesUseCase
{
    private readonly IBaggageRecordRepository _repository;

    public GetBaggageSurchargesUseCase(IBaggageRecordRepository repository)
    {
        _repository = repository;
    }

    public Task<IReadOnlyList<BaggageRecordView>> ExecuteAsync()
    {
        return _repository.GetWithSurchargesAsync();
    }
}
