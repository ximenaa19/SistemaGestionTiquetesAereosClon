// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PassengerTypes\Domain\Aggregate\PassengerType.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PassengerTypes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.PassengerTypes.Domain.Aggregate;

public class PassengerType
{
    public PassengerTypeId Id { get; private set; }
    public PassengerTypeName Name { get; private set; }
    public int? AgeMin { get; private set; }
    public int? AgeMax { get; private set; }

    private PassengerType(PassengerTypeId id, PassengerTypeName name, int? ageMin, int? ageMax)
    {
        Id = id;
        Name = name;
        AgeMin = ageMin;
        AgeMax = ageMax;
    }

    public static PassengerType Create(PassengerTypeId id, PassengerTypeName name, int? ageMin, int? ageMax)
    {
        return new PassengerType(id, name, ageMin, ageMax);
    }

    public static PassengerType CreateNew(PassengerTypeName name, int? ageMin, int? ageMax)
    {
        return new PassengerType(PassengerTypeId.CreateEmpty(), name, ageMin, ageMax);
    }
}

