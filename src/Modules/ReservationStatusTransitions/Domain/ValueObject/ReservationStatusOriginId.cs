// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationStatusTransitions\Domain\ValueObject\ReservationStatusOriginId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.ReservationStatusTransitions.Domain.ValueObject;

public sealed record ReservationStatusOriginId
{
    public int Value { get; }

    private ReservationStatusOriginId(int value)
    {
        Value = value;
    }

    public static ReservationStatusOriginId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El valor no puede ser menor a 1");

        return new ReservationStatusOriginId(value);
    }
}
