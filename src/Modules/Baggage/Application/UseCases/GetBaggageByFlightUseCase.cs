using GestionAerolineas.src.Modules.Baggage.Domain.Models;
using GestionAerolineas.src.Modules.Baggage.Domain.Repositories;

namespace GestionAerolineas.src.Modules.Baggage.Application.UseCases;

// caso de uso para consultar equipaje registrado en un vuelo especifico
public class GetBaggageByFlightUseCase
{
    // repositorio de dominio que abstrae la consulta a la base de datos
    private readonly IBaggageRecordRepository _repository;

    public GetBaggageByFlightUseCase(IBaggageRecordRepository repository)
    {
        // inyecta el repositorio para usarlo en ExecuteAsync
        _repository = repository;
    }

    public Task<IReadOnlyList<BaggageRecordView>> ExecuteAsync(int flightId)
    {
        // evita consultar con un id vacio, cero o negativo
        if (flightId <= 0)
            throw new ArgumentException("El vuelo es obligatorio.");

        // obtiene los registros filtrados por vuelo
        return _repository.GetByFlightIdAsync(flightId);
    }
}
