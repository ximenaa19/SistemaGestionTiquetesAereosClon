// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Sessions\Domain\ValueObject\SessionIsActive.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Sessions.Domain.ValueObject;

public sealed record SessionIsActive
{
    public bool Value { get; }

    private SessionIsActive(bool value)
    {
        Value = value;
    }

    public static SessionIsActive Create(bool value)
    {
        return new SessionIsActive(value);
    }
}
