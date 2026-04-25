// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\InvoiceItemTypes\Application\Interfaces\IInvoiceItemTypeValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.InvoiceItemTypes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.InvoiceItemTypes.Application.Interfaces;

public interface IInvoiceItemTypeValidator
{
    Task ValidateNameAsync(InvoiceItemTypeName name, InvoiceItemTypeId? currentId = null);
}
