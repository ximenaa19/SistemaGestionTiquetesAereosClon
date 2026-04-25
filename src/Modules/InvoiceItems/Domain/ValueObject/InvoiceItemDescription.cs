// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\InvoiceItems\Domain\ValueObject\InvoiceItemDescription.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.InvoiceItems.Domain.ValueObject;

public record InvoiceItemDescription(string Value)
{
    public static InvoiceItemDescription Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("descripcion es obligatoria");

        var trimmed = value.Trim();
        if (trimmed.Length > 200)
            throw new ArgumentException("descripcion excede 200 caracteres");

        return new InvoiceItemDescription(trimmed);
    }
}

