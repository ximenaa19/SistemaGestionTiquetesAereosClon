// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\RoadTypes\Domain\ValueObject\RoadTypeName.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.RoadTypes.Domain.ValueObject;

public sealed record RoadTypeName
{
    public string Value { get; }

    private RoadTypeName(string value)
    {
        Value = value;
    }

    public static RoadTypeName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("El valor no puede estar vacío");
        }

        return new RoadTypeName(value);
    }
    public override string ToString() => Value;
}
