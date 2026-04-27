using GestionAerolineas.src.Modules.Baggage.Domain.Models;
using GestionAerolineas.src.Modules.Baggage.Domain.Repositories;

namespace GestionAerolineas.src.Modules.Baggage.Application.UseCases;

// caso de uso para listar las clases/cabinas disponibles al registrar equipaje
public class GetCabinTypesForBaggageUseCase
{
    // repositorio que consulta las cabinas desde la tabla CabinTypes
    private readonly IBaggageRecordRepository _repository;

    public GetCabinTypesForBaggageUseCase(IBaggageRecordRepository repository)
    {
        // almacena la dependencia para usarla cuando el menu solicite las cabinas
        _repository = repository;
    }

    public Task<IReadOnlyList<CabinTypeOption>> ExecuteAsync()
    {
        // retorna opciones simples con id y nombre para imprimirlas en consola
        return _repository.GetCabinTypesAsync();
    }
}
