// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Reservations\Domain\ValueObject\ReservationCreatedAt.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Reservations.Domain.ValueObject;

public sealed record ReservationCreatedAt
{
    public DateTime? Value { get; }

    private ReservationCreatedAt(DateTime? value)
    {
        Value = value;
    }

    public static ReservationCreatedAt CreateOptional(DateTime? value)
    {
        return new ReservationCreatedAt(value);
    }
}

