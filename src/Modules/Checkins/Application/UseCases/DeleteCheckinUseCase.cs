// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Checkins\Application\UseCases\DeleteCheckinUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Checkins.Domain.Repositories;
using GestionAerolineas.src.Modules.Checkins.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Checkins.Application.UseCases;

public class DeleteCheckinUseCase
{
    private readonly ICheckinRepository _repository;

    public DeleteCheckinUseCase(ICheckinRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var idVO = CheckinId.Create(id);
        var existing = await _repository.GetByIdAsync(idVO);
        if (existing is null)
            throw new Exception("El check-in no existe");

        await _repository.DeleteAsync(existing);
    }
}

