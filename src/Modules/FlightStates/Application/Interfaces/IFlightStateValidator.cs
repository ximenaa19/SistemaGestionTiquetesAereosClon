// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightStates\Application\Interfaces\IFlightStateValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.FlightStates.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.FlightStates.Application.Interfaces;

public interface IFlightStateValidator
{
    Task ValidateNameAsync(FlightStateName name, FlightStateId? currentId = null);
}
