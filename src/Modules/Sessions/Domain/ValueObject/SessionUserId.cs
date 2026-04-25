// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Sessions\Domain\ValueObject\SessionUserId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Sessions.Domain.ValueObject;

public sealed record SessionUserId
{
    public int Value { get; }

    private SessionUserId(int value)
    {
        Value = value;
    }

    public static SessionUserId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El user_id debe ser mayor a 0");

        return new SessionUserId(value);
    }
}
