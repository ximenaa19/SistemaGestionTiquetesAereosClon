using GestionAerolineas.src.Modules.Baggage.Domain.Models;
using GestionAerolineas.src.Modules.Baggage.Domain.Repositories;

namespace GestionAerolineas.src.Modules.Baggage.Application.UseCases;

// caso de uso para consultar todos los registros de equipaje asociados a un cliente
public class GetBaggageByCustomerUseCase
{
    // se usa el contrato del repositorio para no depender directamente de Entity Framework
    private readonly IBaggageRecordRepository _repository;

    public GetBaggageByCustomerUseCase(IBaggageRecordRepository repository)
    {
        // guarda la dependencia que hara la consulta real en infraestructura
        _repository = repository;
    }

    public Task<IReadOnlyList<BaggageRecordView>> ExecuteAsync(int customerId)
    {
        // valida que el id del cliente sea valido antes de consultar
        if (customerId <= 0)
            throw new ArgumentException("El cliente es obligatorio.");

        // delega la consulta al repositorio y retorna una vista lista para mostrar en consola
        return _repository.GetByCustomerIdAsync(customerId);
    }
}
