// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Passengers\Domain\Aggregate\Passenger.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Passengers.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Passengers.Domain.Aggregate;

public class Passenger
{
    public PassengerId Id { get; private set; }
    public PassengerPersonId PersonId { get; private set; }
    public PassengerTypeId PassengerTypeId { get; private set; }

    private Passenger(PassengerId id, PassengerPersonId personId, PassengerTypeId passengerTypeId)
    {
        Id = id;
        PersonId = personId;
        PassengerTypeId = passengerTypeId;
    }

    public static Passenger Create(PassengerId id, PassengerPersonId personId, PassengerTypeId passengerTypeId)
    {
        return new Passenger(id, personId, passengerTypeId);
    }

    public static Passenger CreateNew(PassengerPersonId personId, PassengerTypeId passengerTypeId)
    {
        return new Passenger(PassengerId.CreateEmpty(), personId, passengerTypeId);
    }
}
