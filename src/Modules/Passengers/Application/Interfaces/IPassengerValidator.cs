// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Passengers\Application\Interfaces\IPassengerValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Passengers.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Passengers.Application.Interfaces;

public interface IPassengerValidator
{
    Task ValidatePersonExistsAsync(PassengerPersonId personId);
    Task ValidatePassengerTypeExistsAsync(PassengerTypeId passengerTypeId);
    Task ValidatePersonIsUniqueAsync(PassengerPersonId personId, PassengerId? currentId = null);
}
