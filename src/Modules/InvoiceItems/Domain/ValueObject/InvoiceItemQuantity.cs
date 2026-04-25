// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\InvoiceItems\Domain\ValueObject\InvoiceItemQuantity.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.InvoiceItems.Domain.ValueObject;

public record InvoiceItemQuantity(int Value)
{
    public static InvoiceItemQuantity Create(int value)
    {
        if (value < 1)
            throw new ArgumentException("cantidad debe ser >= 1");
        return new InvoiceItemQuantity(value);
    }
}

