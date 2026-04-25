// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Seasons\Domain\Aggregate\Season.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Seasons.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Seasons.Domain.Aggregate;

public class Season
{
    public SeasonId Id { get; private set; }
    public SeasonName Name { get; private set; }
    public SeasonDescription Description { get; private set; }
    public SeasonPriceFactor PriceFactor { get; private set; }

    private Season(SeasonId id, SeasonName name, SeasonDescription description, SeasonPriceFactor priceFactor)
    {
        Id = id;
        Name = name;
        Description = description;
        PriceFactor = priceFactor;
    }

    public static Season Create(SeasonId id, SeasonName name, SeasonDescription description, SeasonPriceFactor priceFactor)
    {
        return new Season(id, name, description, priceFactor);
    }

    public static Season CreateNew(SeasonName name, SeasonDescription description, SeasonPriceFactor priceFactor)
    {
        return new Season(SeasonId.CreateEmpty(), name, description, priceFactor);
    }
}
