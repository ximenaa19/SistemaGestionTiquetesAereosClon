// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Sessions\Application\UseCases\GetAllSessionsUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Sessions.Domain.Aggregate;
using GestionAerolineas.src.Modules.Sessions.Domain.Repositories;

namespace GestionAerolineas.src.Modules.Sessions.Application.UseCases;

public class GetAllSessionsUseCase
{
    private readonly ISessionRepository _repository;

    public GetAllSessionsUseCase(ISessionRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Session>> ExecuteAsync()
    {
        return _repository.GetAllAsync();
    }
}
