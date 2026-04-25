// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightStates\Domain\Aggregate\FlightState.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.FlightStates.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.FlightStates.Domain.Aggregate;

public class FlightState
{
    public FlightStateId Id { get; private set; }
    public FlightStateName Name { get; private set; }

    private FlightState(FlightStateId id, FlightStateName name)
    {
        Id = id;
        Name = name;
    }

    public static FlightState Create(FlightStateId id, FlightStateName name)
    {
        return new FlightState(id, name);
    }

    public static FlightState CreateNew(FlightStateName name)
    {
        return new FlightState(FlightStateId.CreateEmpty(), name);
    }
}
