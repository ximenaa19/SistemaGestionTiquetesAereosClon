// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\CabinTypes\Domain\Aggregate\CabinType.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System;
using GestionAerolineas.src.Modules.CabinTypes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.CabinTypes.Domain.Aggregate;

public class CabinType
{
    public CabinTypesId Id { get; private set; }
    public CabinTypesName Name { get; private set; }
    

    private CabinType(CabinTypesId id, CabinTypesName name)
    {
        Id = id;
        Name = name;
        
    }

    public static CabinType Create(CabinTypesId id, CabinTypesName name)
    {

        return new CabinType(id, name);
    }

}
