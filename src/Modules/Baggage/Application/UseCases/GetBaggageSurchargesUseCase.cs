using GestionAerolineas.src.Modules.Baggage.Domain.Models;
using GestionAerolineas.src.Modules.Baggage.Domain.Repositories;

namespace GestionAerolineas.src.Modules.Baggage.Application.UseCases;

// caso de uso para listar solo los registros que tuvieron recargo aplicado
public class GetBaggageSurchargesUseCase
{
    // dependencia hacia el contrato del repositorio
    private readonly IBaggageRecordRepository _repository;

    public GetBaggageSurchargesUseCase(IBaggageRecordRepository repository)
    {
        // guarda el repositorio recibido por constructor
        _repository = repository;
    }

    public Task<IReadOnlyList<BaggageRecordView>> ExecuteAsync()
    {
        // no recibe parametros porque el filtro es fijo: recargo_total mayor que cero
        return _repository.GetWithSurchargesAsync();
    }
}
