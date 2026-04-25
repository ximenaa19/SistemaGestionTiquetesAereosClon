// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Users\Application\UseCases\GetUsersByRoleIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Users.Domain.Aggregate;
using GestionAerolineas.src.Modules.Users.Domain.Repositories;
using GestionAerolineas.src.Modules.Users.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Users.Application.UseCases;

public class GetUsersByRoleIdUseCase
{
    private readonly IUserRepository _repository;

    public GetUsersByRoleIdUseCase(IUserRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<User>> ExecuteAsync(int roleId)
    {
        return _repository.GetByRoleIdAsync(UserRoleId.Create(roleId));
    }
}
