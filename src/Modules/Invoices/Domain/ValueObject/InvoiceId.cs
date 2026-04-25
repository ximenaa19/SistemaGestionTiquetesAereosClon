// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Invoices\Domain\ValueObject\InvoiceId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Invoices.Domain.ValueObject;

public record InvoiceId(int Value)
{
    public static InvoiceId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El id de la factura no es valido");
        return new InvoiceId(value);
    }

    public static InvoiceId CreateEmpty() => new(0);
}

