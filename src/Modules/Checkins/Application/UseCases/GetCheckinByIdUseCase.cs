// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Checkins\Application\UseCases\GetCheckinByIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Checkins.Domain.Aggregate;
using GestionAerolineas.src.Modules.Checkins.Domain.Repositories;
using GestionAerolineas.src.Modules.Checkins.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Checkins.Application.UseCases;

public class GetCheckinByIdUseCase
{
    private readonly ICheckinRepository _repository;

    public GetCheckinByIdUseCase(ICheckinRepository repository)
    {
        _repository = repository;
    }

    public Task<Checkin?> ExecuteAsync(int id)
    {
        return _repository.GetByIdAsync(CheckinId.Create(id));
    }
}

