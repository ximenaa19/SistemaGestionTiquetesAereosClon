// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\InvoiceItems\Domain\ValueObject\InvoiceItemUnitPrice.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.InvoiceItems.Domain.ValueObject;

public record InvoiceItemUnitPrice(decimal Value)
{
    public static InvoiceItemUnitPrice Create(decimal value)
    {
        if (value < 0)
            throw new ArgumentException("precio_unitario no puede ser negativo");
        return new InvoiceItemUnitPrice(value);
    }
}

