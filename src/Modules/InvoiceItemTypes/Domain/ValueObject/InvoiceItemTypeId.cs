// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\InvoiceItemTypes\Domain\ValueObject\InvoiceItemTypeId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.InvoiceItemTypes.Domain.ValueObject;

public sealed record InvoiceItemTypeId
{
    public int Value { get; }

    private InvoiceItemTypeId(int value)
    {
        Value = value;
    }

    public static InvoiceItemTypeId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El valor no puede ser menor a 1");

        return new InvoiceItemTypeId(value);
    }

    public static InvoiceItemTypeId CreateEmpty()
    {
        return new InvoiceItemTypeId(0);
    }
}
