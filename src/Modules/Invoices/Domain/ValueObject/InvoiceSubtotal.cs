// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Invoices\Domain\ValueObject\InvoiceSubtotal.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Invoices.Domain.ValueObject;

public record InvoiceSubtotal(decimal Value)
{
    public static InvoiceSubtotal Create(decimal value)
    {
        if (value < 0)
            throw new ArgumentException("subtotal no puede ser negativo");
        return new InvoiceSubtotal(value);
    }
}

