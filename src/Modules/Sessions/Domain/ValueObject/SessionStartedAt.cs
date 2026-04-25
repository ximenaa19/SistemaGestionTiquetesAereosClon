// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Sessions\Domain\ValueObject\SessionStartedAt.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Sessions.Domain.ValueObject;

public sealed record SessionStartedAt
{
    public DateTime Value { get; }

    private SessionStartedAt(DateTime value)
    {
        Value = value;
    }

    public static SessionStartedAt Create(DateTime? value)
    {
        if (!value.HasValue)
            throw new ArgumentException("La fecha de inicio es obligatoria");

        return new SessionStartedAt(value.Value);
    }
}
