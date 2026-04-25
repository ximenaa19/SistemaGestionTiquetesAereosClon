// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Customers\Application\Interfaces\ICustomerValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Customers.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Customers.Application.Interfaces;

public interface ICustomerValidator
{
    Task ValidatePersonExistsAsync(CustomerPersonId personId);
    Task ValidatePersonIsUniqueAsync(CustomerPersonId personId, CustomerId? currentId = null);
}
