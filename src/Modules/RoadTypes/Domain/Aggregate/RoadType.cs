// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\RoadTypes\Domain\Aggregate\RoadType.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System;
using GestionAerolineas.src.Modules.RoadTypes.Domain.ValueObject;


namespace GestionAerolineas.src.Modules.RoadTypes.Domain.Aggregate;

public class RoadType
{
    public RoadTypeId Id { get; private set; }
    public RoadTypeName Name { get; private set; }

    private RoadType(RoadTypeId id, RoadTypeName name)
    {
        Id = id;
        Name = name;
    }

    public static RoadType Create(RoadTypeId id, RoadTypeName name)
    {
        return new RoadType(id, name);
    }
   

}
