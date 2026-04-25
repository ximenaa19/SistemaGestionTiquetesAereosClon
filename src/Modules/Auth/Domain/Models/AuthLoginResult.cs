// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Auth\Domain\Models\AuthLoginResult.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Auth.Application.Models;

public sealed record AuthLoginResult(
    int UserId,
    string Username,
    int RoleId,
    bool IsActive
);

