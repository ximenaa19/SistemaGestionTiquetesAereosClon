// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Users\Application\UseCases\DeleteUserHardUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Users.Domain.Repositories;
using GestionAerolineas.src.Modules.Users.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Users.Application.UseCases;

public class DeleteUserHardUseCase
{
    private readonly IUserRepository _repository;

    public DeleteUserHardUseCase(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(UserId.Create(id));
        if (entity is null)
            throw new Exception("El user no existe");

        await _repository.DeleteAsync(entity);
    }
}
