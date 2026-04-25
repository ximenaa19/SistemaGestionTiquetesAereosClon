// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Invoices\Domain\ValueObject\InvoiceIssuedAt.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Invoices.Domain.ValueObject;

public record InvoiceIssuedAt(DateTime Value)
{
    public static InvoiceIssuedAt Create(DateTime value)
    {
        if (value == default)
            throw new ArgumentException("fecha_emision no es valida");
        return new InvoiceIssuedAt(value);
    }
}

