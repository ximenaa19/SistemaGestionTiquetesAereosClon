// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Customers\Domain\ValueObject\CustomerPersonId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Customers.Domain.ValueObject;

public sealed record CustomerPersonId
{
    public int Value { get; }

    private CustomerPersonId(int value)
    {
        Value = value;
    }

    public static CustomerPersonId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El valor no puede ser menor a 1");

        return new CustomerPersonId(value);
    }
}
