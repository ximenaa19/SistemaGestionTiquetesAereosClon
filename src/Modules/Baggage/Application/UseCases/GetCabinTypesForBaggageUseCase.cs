using GestionAerolineas.src.Modules.Baggage.Domain.Models;
using GestionAerolineas.src.Modules.Baggage.Domain.Repositories;

namespace GestionAerolineas.src.Modules.Baggage.Application.UseCases;

public class GetCabinTypesForBaggageUseCase
{
    private readonly IBaggageRecordRepository _repository;

    public GetCabinTypesForBaggageUseCase(IBaggageRecordRepository repository)
    {
        _repository = repository;
    }

    public Task<IReadOnlyList<CabinTypeOption>> ExecuteAsync()
    {
        return _repository.GetCabinTypesAsync();
    }
}
