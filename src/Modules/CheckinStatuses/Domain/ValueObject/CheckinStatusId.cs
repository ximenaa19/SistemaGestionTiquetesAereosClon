// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\CheckinStatuses\Domain\ValueObject\CheckinStatusId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.CheckinStatuses.Domain.ValueObject;

public sealed record CheckinStatusId
{
    public int Value { get; }

    private CheckinStatusId(int value)
    {
        Value = value;
    }

    public static CheckinStatusId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El valor no puede ser menor a 1");

        return new CheckinStatusId(value);
    }

    public static CheckinStatusId CreateEmpty()
    {
        return new CheckinStatusId(0);
    }
}
