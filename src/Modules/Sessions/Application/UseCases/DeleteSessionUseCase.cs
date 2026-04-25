// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Sessions\Application\UseCases\DeleteSessionUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Sessions.Domain.Repositories;
using GestionAerolineas.src.Modules.Sessions.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Sessions.Application.UseCases;

public class DeleteSessionUseCase
{
    private readonly ISessionRepository _repository;

    public DeleteSessionUseCase(ISessionRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(SessionId.Create(id));
        if (entity is null)
            throw new Exception("La session no existe");

        await _repository.DeleteAsync(entity);
    }
}
