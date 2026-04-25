// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Customers\Domain\ValueObject\CustomerCreatedAt.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Customers.Domain.ValueObject;

public sealed record CustomerCreatedAt
{
    public DateTime Value { get; }

    private CustomerCreatedAt(DateTime value)
    {
        Value = value;
    }

    public static CustomerCreatedAt Create(DateTime value)
    {
        return new CustomerCreatedAt(value);
    }

    public static CustomerCreatedAt Create(DateTime? value)
    {
        return new CustomerCreatedAt(value ?? DateTime.Now);
    }

    public static CustomerCreatedAt CreateNow()
    {
        return new CustomerCreatedAt(DateTime.Now);
    }
}
