// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\InvoiceItems\Domain\ValueObject\InvoiceItemTypeId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.InvoiceItems.Domain.ValueObject;

public record InvoiceItemTypeId(int Value)
{
    public static InvoiceItemTypeId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("tipo_item_id no es valido");
        return new InvoiceItemTypeId(value);
    }
}

