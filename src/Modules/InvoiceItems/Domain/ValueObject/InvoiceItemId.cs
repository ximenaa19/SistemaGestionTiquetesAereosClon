// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\InvoiceItems\Domain\ValueObject\InvoiceItemId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.InvoiceItems.Domain.ValueObject;

public record InvoiceItemId(int Value)
{
    public static InvoiceItemId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El id del item no es valido");
        return new InvoiceItemId(value);
    }

    public static InvoiceItemId CreateEmpty() => new(0);
}

