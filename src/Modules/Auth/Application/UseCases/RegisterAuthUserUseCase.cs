// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Auth\Application\UseCases\RegisterAuthUserUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Users.Application.UseCases;

namespace GestionAerolineas.src.Modules.Auth.Application.UseCases;

/// <summary>
/// Caso de uso de registro rápido desde menú de autenticación.
/// Delega en el caso de creación de usuario para persistir credenciales y rol.
/// </summary>
public class RegisterAuthUserUseCase
{
    private readonly CreateUserUseCase _createUserUseCase;

    public RegisterAuthUserUseCase(
        CreateUserUseCase createUserUseCase)
    {
        _createUserUseCase = createUserUseCase;
    }

    /// <summary>
    /// Registra un nuevo usuario de autenticación.
    /// </summary>
    /// <param name="username">Nombre de usuario único.</param>
    /// <param name="plainPassword">Contraseña en texto plano (se hashea internamente).</param>
    /// <param name="roleId">Rol de sistema seleccionado.</param>
    public async Task ExecuteAsync(string username, string plainPassword, int roleId)
    {
        await _createUserUseCase.ExecuteAsync(username, plainPassword, personId: null, roleId: roleId);
    }
}
