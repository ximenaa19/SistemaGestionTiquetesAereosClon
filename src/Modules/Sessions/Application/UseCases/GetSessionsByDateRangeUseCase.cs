// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Sessions\Application\UseCases\GetSessionsByDateRangeUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Sessions.Domain.Aggregate;
using GestionAerolineas.src.Modules.Sessions.Domain.Repositories;

namespace GestionAerolineas.src.Modules.Sessions.Application.UseCases;

public class GetSessionsByDateRangeUseCase
{
    private readonly ISessionRepository _repository;

    public GetSessionsByDateRangeUseCase(ISessionRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Session>> ExecuteAsync(DateTime from, DateTime to)
    {
        if (to < from)
            throw new Exception("La fecha final no puede ser menor que la inicial");

        return await _repository.GetByDateRangeAsync(from, to);
    }
}
