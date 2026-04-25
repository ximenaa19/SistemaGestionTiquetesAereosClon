// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Checkins\Application\UseCases\GetAllCheckinsUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Checkins.Domain.Aggregate;
using GestionAerolineas.src.Modules.Checkins.Domain.Repositories;

namespace GestionAerolineas.src.Modules.Checkins.Application.UseCases;

public class GetAllCheckinsUseCase
{
    private readonly ICheckinRepository _repository;

    public GetAllCheckinsUseCase(ICheckinRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Checkin>> ExecuteAsync()
    {
        return _repository.GetAllAsync();
    }
}

