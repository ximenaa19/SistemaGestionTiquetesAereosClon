// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\SeatLocationTypes\Domain\Aggregate\SeatLocationType.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.SeatLocationTypes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.SeatLocationTypes.Domain.Aggregate;

public class SeatLocationType
{
    public SeatLocationTypeId Id { get; private set; }
    public SeatLocationTypeName Name { get; private set; }

    private SeatLocationType(SeatLocationTypeId id, SeatLocationTypeName name)
    {
        Id = id;
        Name = name;
    }

    public static SeatLocationType Create(SeatLocationTypeId id, SeatLocationTypeName name)
    {
        return new SeatLocationType(id, name);
    }

    public static SeatLocationType CreateNew(SeatLocationTypeName name)
    {
        return new SeatLocationType(SeatLocationTypeId.CreateEmpty(), name);
    }
}

