// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Users\Application\UseCases\GetAllUsersUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Users.Domain.Aggregate;
using GestionAerolineas.src.Modules.Users.Domain.Repositories;

namespace GestionAerolineas.src.Modules.Users.Application.UseCases;

public class GetAllUsersUseCase
{
    private readonly IUserRepository _repository;

    public GetAllUsersUseCase(IUserRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<User>> ExecuteAsync()
    {
        return _repository.GetAllAsync();
    }
}
