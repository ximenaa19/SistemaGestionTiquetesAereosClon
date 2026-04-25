// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\AvailabilityStatuses\Domain\ValueObject\AvailabilityStatusId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.AvailabilityStatuses.Domain.ValueObject;

public sealed record AvailabilityStatusId
{
    public int Value { get; }

    private AvailabilityStatusId(int value)
    {
        Value = value;
    }

    public static AvailabilityStatusId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El valor no puede ser menor a 1");

        return new AvailabilityStatusId(value);
    }

    public static AvailabilityStatusId CreateEmpty()
    {
        return new AvailabilityStatusId(0);
    }
}
