using GestionAerolineas.src.Modules.Baggage.Domain.Models;
using GestionAerolineas.src.Modules.Baggage.Domain.Repositories;

namespace GestionAerolineas.src.Modules.Baggage.Application.UseCases;

public class GetBaggageByCustomerUseCase
{
    private readonly IBaggageRecordRepository _repository;

    public GetBaggageByCustomerUseCase(IBaggageRecordRepository repository)
    {
        _repository = repository;
    }

    public Task<IReadOnlyList<BaggageRecordView>> ExecuteAsync(int customerId)
    {
        if (customerId <= 0)
            throw new ArgumentException("El cliente es obligatorio.");

        return _repository.GetByCustomerIdAsync(customerId);
    }
}
