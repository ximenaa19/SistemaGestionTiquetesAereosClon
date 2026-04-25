// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\InvoiceItemTypes\Domain\Aggregate\InvoiceItemType.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.InvoiceItemTypes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.InvoiceItemTypes.Domain.Aggregate;

public class InvoiceItemType
{
    public InvoiceItemTypeId Id { get; private set; }
    public InvoiceItemTypeName Name { get; private set; }

    private InvoiceItemType(InvoiceItemTypeId id, InvoiceItemTypeName name)
    {
        Id = id;
        Name = name;
    }

    public static InvoiceItemType Create(InvoiceItemTypeId id, InvoiceItemTypeName name)
    {
        return new InvoiceItemType(id, name);
    }

    public static InvoiceItemType CreateNew(InvoiceItemTypeName name)
    {
        return new InvoiceItemType(InvoiceItemTypeId.CreateEmpty(), name);
    }
}
