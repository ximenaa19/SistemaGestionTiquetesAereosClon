// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Checkins\Domain\ValueObject\CheckinBaggageWeightKg.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Checkins.Domain.ValueObject;

public record CheckinBaggageWeightKg(decimal Value)
{
    public static CheckinBaggageWeightKg Create(decimal? value)
    {
        var v = value ?? 0m;
        if (v < 0)
            throw new ArgumentException("peso_equipaje_kg no puede ser negativo");
        if (v > 999.99m)
            throw new ArgumentException("peso_equipaje_kg excede el maximo permitido (999.99)");
        return new CheckinBaggageWeightKg(decimal.Round(v, 2));
    }
}

