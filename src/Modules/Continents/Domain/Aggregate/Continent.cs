// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Continents\Domain\Aggregate\Continent.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Continents.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Continents.Domain.Aggregate;

public class Continent
{
    public ContinentId Id { get; private set; }
    public ContinentName Name { get; private set; }

    private Continent(ContinentId id, ContinentName name)
    {
        Id = id;
        Name = name;
    }

    public static Continent Create(ContinentId id, ContinentName name)
    {
        return new Continent(id, name);
    }
}


