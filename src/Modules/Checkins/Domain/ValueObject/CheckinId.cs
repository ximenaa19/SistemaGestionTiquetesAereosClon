// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Checkins\Domain\ValueObject\CheckinId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Checkins.Domain.ValueObject;

public record CheckinId(int Value)
{
    public static CheckinId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El id del check-in no es valido");
        return new CheckinId(value);
    }

    public static CheckinId CreateEmpty() => new(0);
}

