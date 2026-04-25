// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Checkins\Domain\ValueObject\CheckinFlightSeatId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Checkins.Domain.ValueObject;

public record CheckinFlightSeatId(int Value)
{
    public static CheckinFlightSeatId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("asiento_vuelo_id no es valido");
        return new CheckinFlightSeatId(value);
    }
}

